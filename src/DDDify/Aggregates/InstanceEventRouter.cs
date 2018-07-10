﻿using System;
using System.Collections.Generic;

namespace DDDify.Aggregates
{
    /// <inheritdoc />
    /// <summary>
    ///     Routes an event to a configured state handler.
    /// </summary>
    public class InstanceEventRouter : IConfigureInstanceEventRouter
    {
        private readonly Dictionary<Type, Action<object>> _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InstanceEventRouter" /> class.
        /// </summary>
        public InstanceEventRouter() => _handlers = new Dictionary<Type, Action<object>>();

        /// <summary>
        ///     Adds a route for the specified event type to the specified state handler.
        /// </summary>
        /// <param name="event">The event type the route is for.</param>
        /// <param name="handler">The state handler that should be invoked when an event of the specified type is routed.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="event" /> or <paramref name="handler" /> is
        ///     <c>null</c>.
        /// </exception>
        public void ConfigureRoute(Type @event, Action<object> handler)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            _handlers.Add(@event, handler);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Adds a route for the specified event type to the specified state handler.
        /// </summary>
        /// <typeparam name="TEvent">The event type the route is for.</typeparam>
        /// <param name="handler">The state handler that should be invoked when an event of the specified type is routed.</param>
        /// <exception cref="T:System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public void ConfigureRoute<TEvent>(Action<TEvent> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            _handlers.Add(typeof(TEvent), @event => handler((TEvent) @event));
        }

        /// <inheritdoc />
        /// <summary>
        ///     Routes the specified <paramref name="event" /> to a configured state handler, if any.
        /// </summary>
        /// <param name="event">The event to route.</param>
        /// <exception cref="T:System.ArgumentNullException">Thrown when the <paramref name="event" /> is null.</exception>
        public void Route(object @event)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            if (_handlers.TryGetValue(@event.GetType(), out var handler))
            {
                handler(@event);
            }
        }
    }

    /// <summary>
    ///     Configures an instance event router with state handlers events can be routed to.
    /// </summary>
    public interface IConfigureInstanceEventRouter : IInstanceEventRouter
    {
        /// <summary>
        ///     Adds a route for the specified event type to the specified state handler.
        /// </summary>
        /// <param name="event">The event type the route is for.</param>
        /// <param name="handler">The state handler that should be invoked when an event of the specified type is routed.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="event" /> or <paramref name="handler" /> is
        ///     <c>null</c>.
        /// </exception>
        void ConfigureRoute(Type @event, Action<object> handler);

        /// <summary>
        ///     Adds a route for the specified event type to the specified state handler.
        /// </summary>
        /// <typeparam name="TEvent">The event type the route is for.</typeparam>
        /// <param name="handler">The state handler that should be invoked when an event of the specified type is routed.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        void ConfigureRoute<TEvent>(Action<TEvent> handler);
    }

    public interface IInstanceEventRouter
    {
        /// <summary>
        ///     Routes the specified <paramref name="event" /> to a configured state handler, if any.
        /// </summary>
        /// <param name="event">The event to route.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="event" /> is null.</exception>
        void Route(object @event);
    }
}