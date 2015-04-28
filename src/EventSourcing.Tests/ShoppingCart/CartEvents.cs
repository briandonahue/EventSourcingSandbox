using System;

namespace EventSourcing.Tests.ShoppingCart
{
    public interface IAggregateEvent
    {
        Guid AggregateId { get; }
    }

    public class ShoppingCartCreated : IAggregateEvent
    {
        public ShoppingCartCreated(Guid cartId, Guid customerId)
        {
            AggregateId = cartId;
            CustomerId = customerId;
        }

        public Guid AggregateId { get; private set; }

        public Guid CustomerId { get; private set; }

    }

    public class ItemAddedToCart : IAggregateEvent
    {
        public ItemAddedToCart(Guid cartId, Guid itemId, int quantity)
        {
            AggregateId = cartId;
            ItemId = itemId;
            Quantity = quantity;
        }

        public Guid AggregateId { get; set; }
        public Guid ItemId { get; private set; }
        public int Quantity { get; private set; }
    }

    public class ItemRemovedFromCart : IAggregateEvent
    {
        public ItemRemovedFromCart(Guid cartId, Guid itemId)
        {
            AggregateId = cartId;
            ItemId = itemId;
        }

        public Guid AggregateId { get; set; }
        public Guid ItemId { get; private set; }
    }

    public class QuantityChangedForItem : IAggregateEvent
    {
        public QuantityChangedForItem(Guid cartId, Guid itemId, int oldQty, int newQty)
        {
            AggregateId = cartId;
            ItemId = itemId;
            OldQuantity = oldQty;
            NewQuantity = newQty;
        }

        public Guid AggregateId { get; set; }
        public Guid ItemId { get; private set; }
        public int OldQuantity { get; private set; }
        public int NewQuantity { get; private set; }
    }

}