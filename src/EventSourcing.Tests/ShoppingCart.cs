using System;
using System.Collections.Generic;

namespace EventSourcing.Tests
{
    public class ShoppingCart: IAggregate
    {
        private readonly List<IDomainEvent> _uncommittedEvents = new List<IDomainEvent>();
        private readonly Guid _id;

        public ShoppingCart(Guid customerId)
        {
            if(customerId == Guid.Empty) throw new ArgumentException("Customer ID must be a valid GUID");
           _id =  Guid.NewGuid();
            RaiseEvent(new ShoppingCartCreated(_id, customerId));
        }

        private void RaiseEvent(IDomainEvent e)
        {
            _uncommittedEvents.Add(e);
        }

        public void AddItem(Guid itemId, int quantity)
        {
            RaiseEvent(new ItemAddedToCart(itemId, quantity));
        }

        public Guid Id
        {
            get { return _id; }
        }

        public IEnumerable<IDomainEvent> GetUncommittedEvents()
        {
            return _uncommittedEvents;
        }

        public void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }
    }
}