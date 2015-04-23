using System;
using System.Collections.Generic;
using System.Threading;

namespace EventSourcing.Tests
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
    }
}