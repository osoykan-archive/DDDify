﻿using System;
using System.Linq;
using DDDify.Aggregates;
using FluentAssertions;
using KellermanSoftware.CompareNetObjects;

namespace TestBase
{
    public class ScenarioForExisting<TAggregateRoot> where TAggregateRoot : IAggregateChangeTracker
    {
        private TAggregateRoot _aggregateRoot;
        private Action<TAggregateRoot>[] _whens;

        public ScenarioForExisting<TAggregateRoot> Given(Func<TAggregateRoot> aggregateRoot)
        {
            _aggregateRoot = aggregateRoot();
            _aggregateRoot.ClearChanges();
            return this;
        }

        public ScenarioForExisting<TAggregateRoot> When(params Action<TAggregateRoot>[] whens)
        {
            _whens = whens;
            return this;
        }

        public ScenarioForExisting<TAggregateRoot> With(Action<TAggregateRoot> act)
        {
            act(_aggregateRoot);
            return this;
        }

        public ScenarioForExisting<TAggregateRoot> ThenAssert(params object[] events)
        {
            foreach (var action in _whens)
            {
                action(_aggregateRoot);
            }

            var logic = new CompareLogic().Compare(_aggregateRoot.GetChanges().ToArray(), events);
            logic.AreEqual.Should().Be(true, "Expected domain event(s) should be fired");
            return this;
        }

        public ScenarioForExisting<TAggregateRoot> AlsoAssert(Func<TAggregateRoot, bool> expression)
        {
            expression(_aggregateRoot).Should().Be(true, "AlsoAssert is not validated maybe event is not applied");
            return this;
        }

        public void ThenNone()
        {
            foreach (var action in _whens)
            {
                action(_aggregateRoot);
            }

            _aggregateRoot.GetChanges().ToArray().Should()
                .BeEmpty("Aggregate should not have any events! But founded.");
        }

        public void ThenThrows<TException>(string message = "") where TException : Exception
        {
            Action act = () =>
            {
                foreach (var action in _whens)
                {
                    action(_aggregateRoot);
                }
            };

            act.Should().Throw<TException>().WithMessage(message);
        }
    }
}