using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using DDDify.Aggregates;
using DDDify.Repositories;

namespace DDDify.Uow
{
    /// <inheritdoc />
    /// <summary>
    ///     Base for all Unit Of Work classes.
    /// </summary>
    [DebuggerDisplay("Id = {" + nameof(Id) + "}")]
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        public string Id { get; protected set; }

        public abstract Task For<TAggregateRoot, TPrimaryKey>(
            Func<IRepository<TAggregateRoot, TPrimaryKey>, Task> when,
            Action onCompleted,
            Action<Exception> onFailed,
            CancellationToken cancellationToken = default(CancellationToken))
            where TAggregateRoot : class, IAggregateRoot<TPrimaryKey>;
    }
}
