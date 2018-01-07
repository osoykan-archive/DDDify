using System;
using System.Threading;
using System.Threading.Tasks;

using DDDify.Aggregates;
using DDDify.Repositories;

namespace DDDify.Uow
{
    public interface IUnitOfWork
    {
        string Id { get; }

        Task For<TAggregateRoot, TPrimaryKey>(
            Func<IRepository<TAggregateRoot, TPrimaryKey>, Task> when,
            Action<UnitOfWorkOptions> optionsCreator = null,
            Action onCompleted = null,
            Action<Exception> onFailed = null,
            bool throwIfNeeded = true,
            CancellationToken cancellationToken = default)
            where TAggregateRoot : class, IAggregateRoot<TPrimaryKey>;
    }
}
