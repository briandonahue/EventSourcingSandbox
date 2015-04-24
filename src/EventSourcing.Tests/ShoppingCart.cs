using System;
using System.Collections.Generic;

namespace EventSourcing.Tests
{
    public class ShoppingCart: IAggregate
    {
        private readonly List<IAggregateEvent> _uncommittedEvents = new List<IAggregateEvent>();

        public ShoppingCart()
        {
        }

        public ShoppingCart(Guid customerId)
        {
            if(customerId == Guid.Empty) throw new ArgumentException("Customer ID must be a valid GUID");
            ApplyEvent(new ShoppingCartCreated(Guid.NewGuid(), customerId));
        }

        public void AddItem(Guid itemId, int quantity)
        {
            ApplyEvent(new ItemAddedToCart(Id, itemId, quantity));
        }

        public Guid Id { get; private set; }

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

        private void ApplyEvent(IAggregateEvent e, bool uncommitted = true)
        {
            this.AsDynamic().Apply(e);
            if(uncommitted) _uncommittedEvents.Add(e);

        }

        void Apply(ShoppingCartCreated e)
        {
            Id = e.AggregateId;
        }
    }
}