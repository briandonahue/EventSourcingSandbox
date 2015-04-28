using System;
using System.Collections.Generic;
using EventSourcing.Tests.ShoppingCart;

namespace EventSourcing.Tests.Domain
{
    public interface IAggregate
    {
        Guid Id { get; }
        IEnumerable<IAggregateEvent> GetUncommittedEvents();
        void ClearUncommittedEvents();
        void LoadHistory(IEnumerable<IAggregateEvent> events);
    }
}