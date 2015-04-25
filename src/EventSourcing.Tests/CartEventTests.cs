using System;
using Should;

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

        public void WhenRemovingItemFromCartThatHasAlreadyBeenRemoved()
        {
            Given(
                new ShoppingCartCreated(AggregateId, _customerId),
                new ItemAddedToCart(AggregateId, _itemId, 1),
                new ItemRemovedFromCart(AggregateId, _itemId)
            );
            When(x => x.RemoveItem(_itemId));
            ExpectException<ItemNotFoundInCartException>(e => e.ItemId.ShouldEqual(_itemId, "ItemId"));
        }

        public void WhenRemovingItemFromCartThatHasNotBeenAdded()
        {
            Given(
                new ShoppingCartCreated(AggregateId, _customerId)
            );
            When(x => x.RemoveItem(_itemId));
            ExpectException<ItemNotFoundInCartException>(e => e.ItemId.ShouldEqual(_itemId, "ItemId"));
        }

        public void WhenChangingQuantityOfItemInCart()
        {
            var originalQty = 1;
            var newQty = 5;
            Given(
                new ShoppingCartCreated(AggregateId, _customerId),
                new ItemAddedToCart(AggregateId, _itemId, originalQty)
            );
            When(x => x.ChangeQuantity(_itemId, newQty));
            Expect(new QuantityChangedForItem(AggregateId, _itemId, originalQty, newQty));
        }

        public void WhenChangingQuantityOfItemInCartMoreThanOnce()
        {
            var originalQty = 1;
            var firstChange = 5;
            var secondChange = 10;
            Given(
                new ShoppingCartCreated(AggregateId, _customerId),
                new ItemAddedToCart(AggregateId, _itemId, originalQty),
new QuantityChangedForItem(AggregateId, _itemId, originalQty, firstChange)
            );
            When(x => x.ChangeQuantity(_itemId, secondChange));
            Expect(new QuantityChangedForItem(AggregateId, _itemId, firstChange, secondChange));
        }

        public void WhenAttemptingToChangeQuantityOfItemNotInCart()
        {
            var newQty = 5;
            Given(
                new ShoppingCartCreated(AggregateId, _customerId)
            );
            When(x => x.ChangeQuantity(_itemId, newQty));
            ExpectException<ItemNotFoundInCartException>(e => e.ItemId.ShouldEqual(_itemId, "ItemId"));
        }

    }

}