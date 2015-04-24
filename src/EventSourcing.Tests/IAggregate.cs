using System;
using System.Collections.Generic;

namespace EventSourcing.Tests
{
    public interface IAggregate
    {
        Guid Id { get; }
        IEnumerable<IAggregateEvent> GetUncommittedEvents();
        void ClearUncommittedEvents();
        void LoadHistory(IEnumerable<IAggregateEvent> events);
    }
}