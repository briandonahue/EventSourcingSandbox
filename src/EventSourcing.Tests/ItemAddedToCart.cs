using System;

namespace EventSourcing.Tests
{
    public interface IDomainEvent
    {
    }

    public class ShoppingCartCreated : IDomainEvent
    {
        public ShoppingCartCreated(Guid cartId, Guid customerId)
        {
            CartId = cartId;
            CustomerId = customerId;
        }

        public Guid CartId { get; private set; }
        public Guid CustomerId { get; private set; }
    }

    public class ItemAddedToCart : IDomainEvent
    {
        public ItemAddedToCart(Guid itemId, int quantity)
        {
            ItemId = itemId;
            Quantity = quantity;
        }

        public Guid ItemId { get; private set; }
        public int Quantity { get; private set; }
    }
}