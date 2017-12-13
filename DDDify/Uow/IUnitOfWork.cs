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
            Action onCompleted,
            Action<Exception> onFailed, 
            CancellationToken cancellationToken = default(CancellationToken))
            where TAggregateRoot : class, IAggregateRoot<TPrimaryKey>;
    }
}
