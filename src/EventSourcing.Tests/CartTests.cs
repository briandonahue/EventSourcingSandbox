using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EventStore.ClientAPI.Embedded;
using Should;

namespace EventSourcing.Tests
{
    public class CartTests
    {
        private readonly Guid _customerId = Guid.NewGuid();
        private AggregateRepository _repo;
        private InMemoryBus _bus;

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
        }

        public void ShouldDoStuff()
        {
            var cart = new ShoppingCart(_customerId);
            cart.AddItem(Guid.NewGuid(), 1);
            _repo.Save(cart);
            var publishedEvents = _bus.GetAllPublishedEvents();
            var first = publishedEvents.ElementAt(0);
            first.ShouldBeType<ShoppingCartCreated>();

        }

    }
}
