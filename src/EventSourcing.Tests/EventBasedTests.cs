using System;
using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;

namespace EventSourcing.Tests
{
    public class EventBasedTests<T> where T : IAggregate
    {
        private Action<object> _command;
        private IAggregateEvent[] _given;
        private CompareLogic _eventCompareLogic;
        private IAggregateEvent[] _expected;

        public EventBasedTests()
        {
            AggregateId = Guid.NewGuid();
            _eventCompareLogic = new CompareLogic();
        }

        public Guid AggregateId { get; private set; }

        protected void When(Action<T> action)
        {
            _command = (sut) => action((T) sut);
        }

        protected void Given(params IAggregateEvent[] events)
        {
            _given = events;
        }

        protected void Expect(params IAggregateEvent[] events)
        {
            _expected = events;
        }

        protected void ExpectNoEvents()
        {
            _expected = new IAggregateEvent[0];
        }


        private void Run()
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