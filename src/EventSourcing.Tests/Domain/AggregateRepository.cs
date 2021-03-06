﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EventSourcing.Tests.ShoppingCart;

namespace EventSourcing.Tests.Domain
{
    public class AggregateRepository
    {
        private readonly IEventBus _bus;

        public AggregateRepository(IEventBus bus)
        {
            _bus = bus;
        }

        public void Save<T>(T aggregate) where T : IAggregate
        {
            Save(aggregate, null);
        }
        public void Save<T>(T aggregate, Action<IDictionary<string, object>> updateHeaders) where T: IAggregate
        {
            var headers = PrepareHeaders(aggregate, updateHeaders);
            var streamName = "esdemo-" + aggregate.GetType().Name + "-" + aggregate.Id;
            _bus.PublishAsync(streamName, headers, aggregate.GetUncommittedEvents()).Wait();
            aggregate.ClearUncommittedEvents();
        }


        static Dictionary<string, object> PrepareHeaders(IAggregate aggregate,
            Action<IDictionary<string, object>> updateHeaders)
        {
            var headers = new Dictionary<string, object>();
            var user = Thread.CurrentPrincipal;
            if (user != null)
                headers[HeaderKeys.UserName] = user.Identity.Name;

            headers[HeaderKeys.AggregateClrTypeName] = aggregate.GetType().FullName;
            if (updateHeaders != null)
                updateHeaders(headers);

            return headers;
        }

        public T Get<T>(Guid aggregateId) where T:IAggregate
        {
            var aggregate = Activator.CreateInstance<T>();
            var events = _bus.GetEventsFromStream(typeof (T).FullName + "-" + aggregateId).Cast<IAggregateEvent>();
            aggregate.LoadHistory(events);
            return aggregate;
        }
    }
}