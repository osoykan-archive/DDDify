﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using DDDify.Aggregates;
using MediatR;

namespace DDDify.Entities
{
    /// <inheritdoc />
    /// <summary>
    ///     Basic implementation of IEntity interface.
    ///     An entity can inherit this class of directly implement to IEntity interface.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    [Serializable]
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        private readonly InstanceEventRouter _router;

        protected Entity() => _router = new InstanceEventRouter();

        /// <summary>
        ///     Unique identifier for this entity.
        /// </summary>
        [Key]
        public virtual TPrimaryKey Id { get; set; }

        /// <summary>
        ///     Registers the state handler to be invoked when the specified event is applied.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event to register the handler for.</typeparam>
        /// <param name="handler">The handler.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="handler" /> is null.</exception>
        protected void Register<TEvent>(Action<TEvent> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            _router.ConfigureRoute(handler);
        }

        /// <summary>
        ///     Routes the specified <paramref name="event" /> to a configured state handler, if any.
        /// </summary>
        /// <param name="event">The event to route.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="event" /> is null.</exception>
        public virtual void Route(object @event)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            _router.Route(@event);
        }

        #region Overrides

        /// <inheritdoc />
        /// <summary>
        ///     Checks if this entity is transient (it has not an Id).
        /// </summary>
        /// <returns>True, if this entity is transient</returns>
        public virtual bool IsTransient()
        {
            if (EqualityComparer<TPrimaryKey>.Default.Equals(Id, default))
            {
                return true;
            }

            //Workaround for EF Core since it sets int/long to min value when attaching to dbcontext
            if (typeof(TPrimaryKey) == typeof(int))
            {
                return Convert.ToInt32(Id) <= 0;
            }

            if (typeof(TPrimaryKey) == typeof(long))
            {
                return Convert.ToInt64(Id) <= 0;
            }

            return false;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is Entity<TPrimaryKey>))
            {
                return false;
            }

            //Same instances must be considered as equal
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            //Transient objects are not considered as equal
            var other = (Entity<TPrimaryKey>) obj;
            if (IsTransient() && other.IsTransient())
            {
                return false;
            }

            //Must have a IS-A relation of types or must be same type
            var typeOfThis = GetType();
            var typeOfOther = other.GetType();
            if (!typeOfThis.GetTypeInfo().IsAssignableFrom(typeOfOther) &&
                !typeOfOther.GetTypeInfo().IsAssignableFrom(typeOfThis))
            {
                return false;
            }

            return Id.Equals(other.Id);
        }

        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==(Entity<TPrimaryKey> left, Entity<TPrimaryKey> right)
        {
            if (Equals(left, null))
            {
                return Equals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Entity<TPrimaryKey> left, Entity<TPrimaryKey> right) => !(left == right);

        public override string ToString() => $"[{GetType().Name} {Id}]";

        #endregion
    }
}