using System;
using System.Collections.Generic;
using EventSourcing.Tests.ShoppingCart;
using EventSourcing.Tests.Utils;

namespace EventSourcing.Tests.Domain
{
    public class AggregateBase : IAggregate
    {
        public Guid Id { get; protected set; }

        private readonly List<IAggregateEvent> _uncommittedEvents = new List<IAggregateEvent>();

        public IEnumerable<IAggregateEvent> GetUncommittedEvents()
        {
            return _uncommittedEvents;
        }

        public void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }

        public void LoadHistory(IEnumerable<IAggregateEvent> events)
        {
            foreach (var e in events) ApplyEvent(e, false);
        }

        protected void ApplyEvent(IAggregateEvent e, bool uncommitted = true)
        {
            this.AsDynamic().Apply(e);
            if (uncommitted) _uncommittedEvents.Add(e);
        }
    }
}