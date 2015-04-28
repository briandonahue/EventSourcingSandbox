using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EventSourcing.Tests.Domain;
using EventSourcing.Tests.ShoppingCart;
using EventSourcing.Tests.TestHelpers;
using EventStore.ClientAPI.Embedded;
using Should;

namespace EventSourcing.Tests.EventStore
{
    public class CartTests
    {
        private readonly Guid _customerId = Guid.NewGuid();
        private AggregateRepository _repo;
        private InMemoryBus _bus;
        private IEnumerable<object> _publishedEvents;
        private ShoppingCart.ShoppingCart _cart;

        public CartTests()
        {
            // only needed if wanting to run on disk, which has some advantages - don't ask me what, please
            var path = Path.Combine(Path.GetTempPath(), string.Format("{0}-{1}", Guid.NewGuid(), GetType().Name));
            var node = EmbeddedVNodeBuilder.
                AsSingleNode().
                OnDefaultEndpoints().
                RunInMemory().
                Build();
            node.Start();
            var connection = EmbeddedEventStoreConnection.Create(node);

            _bus = new InMemoryBus();
            _repo = new AggregateRepository(_bus);
            _cart = new ShoppingCart.ShoppingCart(_customerId);
            _cart.AddItem(Guid.NewGuid(), 1);
            _repo.Save(_cart);
            _publishedEvents = _bus.GetAllPublishedEvents();
        }

        public void FirstEventShouldBeShoppingCartCreated()
        {
            var first = _publishedEvents.ElementAt(0);
            first.ShouldBeType<ShoppingCartCreated>();
            var cartCreated = (ShoppingCartCreated) first;
            cartCreated.AggregateId.ShouldEqual(_cart.Id);
            cartCreated.CustomerId.ShouldEqual(_customerId);
        }

        public void CartCreatedIdShouldEqualCartId()
        {
            
            var first = _publishedEvents.ElementAt(0);
        }

    }
}
