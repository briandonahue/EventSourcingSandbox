using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventSourcing.Tests.Domain;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace EventSourcing.Tests.EventStore
{
    public class EventStoreBus : IEventBus
    {
        private readonly IEventStoreConnection connection;
        private readonly JsonSerializerSettings serializationSettings;

        public EventStoreBus(IEventStoreConnection connection)
        {
            this.connection = connection;
            serializationSettings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.None};
        }

        public Task PublishAsync(string streamName, IDictionary<string, object> headers, IEnumerable<object> events)
        {
            headers.Add(HeaderKeys.CommitTimestamp, DateTime.UtcNow);
            return connection.AppendToStreamAsync(streamName, ExpectedVersion.Any, ToEventData(events, headers));
        }

        public IEnumerable<object> GetEventsFromStream(string streamName)
        {
            var eventSlice =
                connection.ReadStreamEventsForwardAsync(streamName, StreamPosition.Start, StreamPosition.End, false)
                    .Result;
            return eventSlice.Events.Select(GetTypedEvent);
        }

        private object GetTypedEvent(ResolvedEvent e)
        {
            var originalEvent = e.Event;
            var headerJson = Encoding.UTF8.GetString(originalEvent.Metadata);
            var headers = JsonConvert.DeserializeObject<IDictionary<string, object>>(headerJson);

            if (!headers.ContainsKey(HeaderKeys.EventClrTypeName))
            {
                throw new Exception(string.Format("Event {0} in stream {1} missing header: {2}", e.Event.EventId,
                    e.Event.EventStreamId, HeaderKeys.EventClrTypeName));
            }
            var eventTypeName = headers[HeaderKeys.EventClrTypeName].ToString();
            //  how to match type here?
            var eventType = Type.GetType(eventTypeName);

            var jsonStr = Encoding.UTF8.GetString(originalEvent.Data);
            var typedEvent = JsonConvert.DeserializeObject(jsonStr, eventType);
            return typedEvent;
        }

        private EventData[] ToEventData(IEnumerable<object> events, IDictionary<string, object> headers)
        {
            return events.Select(e =>
            {
                var metadata = new Dictionary<string, object>(headers)
                {
                    {HeaderKeys.EventClrTypeName, e.GetType().FullName}
                };
                var headerJson = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(metadata, serializationSettings));
                var eventJson = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(e, serializationSettings));
                return new EventData(Guid.NewGuid(), e.GetType().Name, true, eventJson, headerJson);
            }).ToArray();
        }

        public void SubscribeAll(Action<PublishedEvent> subscriber)
        {
            throw new NotImplementedException();
        }
    }

    public class PublishedEvent
    {
        public PublishedEvent(IDictionary<string, object> headers, object e)
        {
            Headers = headers;
            Event = e;
        }

        public IDictionary<string, object> Headers { get; private set; }
        public object Event { get; set; }
    }
}