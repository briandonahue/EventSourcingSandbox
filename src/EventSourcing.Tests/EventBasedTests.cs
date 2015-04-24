using System;
using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;

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

        public void Test()
        {
            

            Given(new ShoppingCartCreated(AggregateId, _customerId));
            When(x => x.AddItem(_itemId, 1));
            Expect(new ItemAddedToCart(AggregateId, _itemId, 1));
            Run();
        }
    }

    public class EventBasedTests<T> where T : IAggregate
    {
        private Action<T> _command;
        private IAggregateEvent[] _given;
        private CompareLogic _eventCompareLogic;
        private IAggregateEvent[] _expected;

        public EventBasedTests()
        {
            AggregateId = Guid.NewGuid();
            _eventCompareLogic = new CompareLogic();
        }

        public Guid AggregateId { get; private set; }

        public void When(Action<T> action)
        {
            _command = action;
        }

        public void Given(params IAggregateEvent[] events)
        {
            _given = events;
        }

        public void Expect(params IAggregateEvent[] events)
        {
            _expected = events;
        }

        protected void Run()
        {
            var aggregate = Activator.CreateInstance<T>();
            aggregate.LoadHistory(_given);
            _command(aggregate);
            var uncommittedEvents = aggregate.GetUncommittedEvents().ToArray();
            var comparisonResult = _eventCompareLogic.Compare(_expected, uncommittedEvents);
            if(!comparisonResult.AreEqual) throw new Exception(comparisonResult.DifferencesString);


        }
    }
}