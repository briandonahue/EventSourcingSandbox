using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace EventSourcing.Tests
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
        public IDictionary<string, object> Headers { get; private set; }
        public object Event { get; set; }

        public PublishedEvent(IDictionary<string, object> headers, object e)
        {
            Headers = headers;
            Event = e;
        }
    }
}