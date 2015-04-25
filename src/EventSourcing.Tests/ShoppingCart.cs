using System;
using System.Collections.Generic;

namespace EventSourcing.Tests
{
    public class ShoppingCart : AggregateBase
    {
        private readonly IDictionary<Guid, int> _items = new Dictionary<Guid, int>();

        public ShoppingCart()
        {
        }

        public ShoppingCart(Guid customerId)
        {
            if (customerId == Guid.Empty) throw new ArgumentException("Customer ID must be a valid GUID");
            ApplyEvent(new ShoppingCartCreated(Guid.NewGuid(), customerId));
        }

        public void AddItem(Guid itemId, int quantity)
        {
            ApplyEvent(new ItemAddedToCart(Id, itemId, quantity));
        }

        public void RemoveItem(Guid itemId)
        {
            if (!_items.ContainsKey(itemId)) throw new ItemNotFoundInCartException(itemId);
            ApplyEvent(new ItemRemovedFromCart(Id, itemId));
        }

        private void Apply(ShoppingCartCreated e)
        {
            Id = e.AggregateId;
        }

        private void Apply(ItemAddedToCart e)
        {
            _items.Add(e.ItemId, e.Quantity);
        }

        private void Apply(ItemRemovedFromCart e)
        {
            _items.Remove(e.ItemId);
        }

        private void Apply(QuantityChangedForItem e)
        {
            _items[e.ItemId] = e.NewQuantity;
        }


        public void ChangeQuantity(Guid itemId, int newQty)
        {
            if (!_items.ContainsKey(itemId)) throw new ItemNotFoundInCartException(itemId);
            ApplyEvent(new QuantityChangedForItem(Id, itemId, _items[itemId], newQty));
        }
    }

    public class ItemNotFoundInCartException : Exception
    {
        public ItemNotFoundInCartException(Guid itemId)
        {
            ItemId = itemId;
        }

        public Guid ItemId { get; private set; }
    }
}