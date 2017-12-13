using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using DDDify.Aggregates;

namespace DDDify.Repositories
{
    /// <inheritdoc />
    /// <summary>
    ///     Base class to implement <see cref="T:DDDify.Repositories.IRepository`2" />.
    ///     It implements some methods in most simple way.
    /// </summary>
    /// <typeparam name="TAggregateRoot">Type of the Entity for this repository</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key of the entity</typeparam>
    public abstract class RepositoryBase<TAggregateRoot, TPrimaryKey> : IRepository<TAggregateRoot, TPrimaryKey>
        where TAggregateRoot : class, IAggregateRoot<TPrimaryKey>
    {
        public abstract IQueryable<TAggregateRoot> GetAll();

        public abstract Task<TAggregateRoot> Get(TPrimaryKey id, CancellationToken cancellationToken = default(CancellationToken));

        public abstract Task<TAggregateRoot> Get(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));

        public abstract Task Insert(TAggregateRoot entity, CancellationToken cancellationToken = default(CancellationToken));

        public abstract Task Update(TAggregateRoot entity, CancellationToken cancellationToken = default(CancellationToken));

        public async Task Update(TPrimaryKey id, Func<TAggregateRoot, Task> updateAction, CancellationToken cancellationToken = default(CancellationToken))
        {
            TAggregateRoot aggregate = await Get(id, cancellationToken);
            await updateAction(aggregate);
            await Update(aggregate, cancellationToken);
        }

        public abstract Task Delete(TAggregateRoot entity, CancellationToken cancellationToken = default(CancellationToken));

        public async Task Delete(TPrimaryKey id, CancellationToken cancellationToken = default(CancellationToken))
        {
            TAggregateRoot aggregate = await Get(id, cancellationToken);
            await Delete(aggregate, cancellationToken);
        }

        public async Task Delete(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (TAggregateRoot aggregate in GetAll().Where(predicate))
            {
                await Delete(aggregate, cancellationToken);
            }
        }
    }
}
