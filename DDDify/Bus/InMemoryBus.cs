using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DDDify.Bus;

namespace DDDify
{
    public class InMemoryBus : IBus
    {
        private readonly Dictionary<Type, List<Action<IMessage>>> _routes = new Dictionary<Type, List<Action<IMessage>>>();

        public Task Send<T>(T command) where T : Command
        {
            if (_routes.TryGetValue(typeof(T), out List<Action<IMessage>> handlers))
            {
                if (handlers.Count != 1)
                {
                    throw new InvalidOperationException("Command Handler cannot be more than one");
                }

                handlers.First()(command);
            }
            else
            {
                throw new InvalidOperationException("no handler registered");
            }

            return Task.CompletedTask;
        }

        public Task Publish<T>(T @event) where T : Event
        {
            if (!_routes.TryGetValue(@event.GetType(), out List<Action<IMessage>> handlers))
            {
                return Task.CompletedTask;
            }

            foreach (Action<IMessage> handler in handlers)
            {
                ThreadPool.QueueUserWorkItem(state => handler(@event));
            }

            return Task.CompletedTask;
        }

        public Task Register<T>(Action<T> handler)
        {
            if (!_routes.TryGetValue(typeof(T), out List<Action<IMessage>> handlers))
            {
                handlers = new List<Action<IMessage>>();
                _routes.Add(typeof(T), handlers);
            }

            handlers.Add(message => handler((T)message));

            return Task.CompletedTask;
        }
    }
}
