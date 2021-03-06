﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSourcing.Tests.Domain;
using EventSourcing.Tests.EventStore;
using EventSourcing.Tests.ShoppingCart;

namespace EventSourcing.Tests.TestHelpers
{
    public class InMemoryBus: IEventBus
    {
        IDictionary<string, List<PublishedEvent>>_events = new Dictionary<string, List<PublishedEvent>>();
        IList<Action<PublishedEvent>> _subscriberActions = new List<Action<PublishedEvent>>();


        public Task PublishAsync(string streamName, IDictionary<string, object> headers, IEnumerable<object> events)
        {
            if(!_events.ContainsKey(streamName)) _events.Add(streamName, new List<PublishedEvent>());
            _events[streamName].AddRange(events.Select(e => new PublishedEvent(null, e)));
            return Task.FromResult(0);

        }

        public IEnumerable<object> GetEventsFromStream(string streamName)
        {
            return _events.ContainsKey(streamName) ? _events[streamName].Select(p => p.Event) : new List<object>();
        }

        public IEnumerable<object> GetAllPublishedEvents()
        {
            return _events.Values.SelectMany(x => x).Select(p => p.Event);
        }

        public void Publish(string streamName, IAggregateEvent[] events)
        {
            PublishAsync(streamName, null, events);
        }
    }
}