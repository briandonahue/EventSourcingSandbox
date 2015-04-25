using System;

namespace EventSourcing.Tests
{
    public class CartEventTests : EventBasedTests<ShoppingCart>
    {
        private ShoppingCart _cart;
        private readonly Guid _customerId;
        private readonly Guid _itemId;

        public CartEventTests()
        {
            _customerId = Guid.NewGuid();
            _itemId = Guid.NewGuid();

        }

        public void WhenAddingAnItemToACart()
        {
            Given(new ShoppingCartCreated(AggregateId, _customerId));
            When(x => x.AddItem(_itemId, 1));
            Expect(new ItemAddedToCart(AggregateId, _itemId, 1));
        }

        public void WhenRemovingItemFromCart()
        {
            Given(
                new ShoppingCartCreated(AggregateId, _customerId),
                new ItemAddedToCart(AggregateId, _itemId, 1)
            );
            When(x => x.RemoveItem(_itemId));
            Expect(new ItemRemovedFromCart(AggregateId, _itemId));
        }

        public void WhenRemovingItemFromCartThatHasNotBeenAdded()
        {
            Given(
                new ShoppingCartCreated(AggregateId, _customerId)
            );
            When(x => x.RemoveItem(_itemId));
            ExpectNoEvents();
        }
    }
}