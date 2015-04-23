using System;
using System.Collections.Generic;

namespace EventSourcing.Tests
{
    public interface IAggregate
    {
        Guid Id { get; }
        IEnumerable<IDomainEvent> GetUncommittedEvents();
        void ClearUncommittedEvents();
    }
}