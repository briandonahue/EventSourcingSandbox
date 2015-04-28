using System;
using System.Linq;
using EventSourcing.Tests.Domain;
using EventSourcing.Tests.ShoppingCart;
using KellermanSoftware.CompareNetObjects;

namespace EventSourcing.Tests.TestHelpers
{
    public class EventBasedTests<T> where T : IAggregate
    {
        private Action<object> _command;
        private Action<Exception> _exceptionValidator = exception => { };
        private IAggregateEvent[] _expected;
        private Type _expectedExceptionType;
        private IAggregateEvent[] _given;
        private readonly CompareLogic _eventCompareLogic;

        public EventBasedTests()
        {
            AggregateId = Guid.NewGuid();
            _eventCompareLogic = new CompareLogic();
        }

        public Guid AggregateId { get; private set; }

        protected void When(Action<T> action)
        {
            _command = sut => action((T) sut);
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
            try
            {
                _command(aggregate);
                if (_expectedExceptionType != null) throw new NoExceptionException(_expectedExceptionType);
            }
            catch (Exception ex)
            {
                if (_expectedExceptionType == null || _expectedExceptionType != ex.GetType()) throw;
                _exceptionValidator(ex);
                return;
            }
            var uncommittedEvents = aggregate.GetUncommittedEvents().ToArray();
            var comparisonResult = _eventCompareLogic.Compare(_expected, uncommittedEvents);
            if (!comparisonResult.AreEqual) throw new Exception(comparisonResult.DifferencesString);
        }

        protected void ExpectException<TException>(Action<TException> exceptionValidation = null)
            where TException : Exception
        {
            _expectedExceptionType = typeof (TException);
            if (exceptionValidation != null)
            {
                _exceptionValidator = ex => exceptionValidation((TException) ex);
            }
        }
    }

    public class NoExceptionException : Exception
    {
        public NoExceptionException(Type expectedExceptionType)
            : base(string.Format("Expected Exception <{0}> was not thrown.", expectedExceptionType.Name))
        {
        }
    }
}