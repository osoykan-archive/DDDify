using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDDify.Bus
{
    public class InMemoryBus : IBus
    {
        private readonly Dictionary<Type, List<Func<IMessage, Task>>> _routes = new Dictionary<Type, List<Func<IMessage, Task>>>();

        public async Task Send<T>(T command) where T : Command
        {
            if (_routes.TryGetValue(typeof(T), out List<Func<IMessage, Task>> handlers))
            {
                if (handlers.Count != 1)
                {
                    throw new InvalidOperationException("Command Handler cannot be more than one");
                }

                await handlers.First()(command);
            }
            else
            {
                throw new InvalidOperationException("no handler registered");
            }
        }

        public async Task Publish<T>(T @event) where T : Event
        {
            if (!_routes.TryGetValue(@event.GetType(), out List<Func<IMessage, Task>> handlers))
            {
                return;
            }

            foreach (Func<IMessage, Task> handler in handlers)
            {
                await handler(@event);
            }
        }

        public Task Register<T>(Func<T, Task> handler)
        {
            if (!_routes.TryGetValue(typeof(T), out List<Func<IMessage, Task>> handlers))
            {
                handlers = new List<Func<IMessage, Task>>();
                _routes.Add(typeof(T), handlers);
            }

            handlers.Add(async message => await handler((T)message));

            return Task.CompletedTask;
        }
    }
}
