using System.Collections.Generic;

namespace DDDify.Aggregates
{
    public interface IAggregateRoot<TPrimaryKey> : IAggregateChangeTracker
    {
        /// <summary>
        ///     Unique identifier for this AggregateRoot.
        /// </summary>
        TPrimaryKey Id { get; set; }

        /// <summary>
        ///     Checks if this entity is transient (not persisted to database and it has not an <see cref="Id" />).
        /// </summary>
        /// <returns>True, if this entity is transient</returns>
        bool IsTransient();
    }

    /// <summary>
    ///     Tracks changes that happen to an aggregate
    /// </summary>
    public interface IAggregateChangeTracker
    {
        /// <summary>
        ///     Determines whether this instance has state changes.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if this instance has state changes; otherwise, <c>false</c>.
        /// </returns>
        bool HasChanges();

        /// <summary>
        ///     Gets the state changes applied to this instance.
        /// </summary>
        /// <returns>A list of recorded state changes.</returns>
        IEnumerable<object> GetChanges();

        /// <summary>
        ///     Clears the state changes.
        /// </summary>
        void ClearChanges();
    }
}