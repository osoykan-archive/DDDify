using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using DDDify.Aggregates;

namespace DDDify.Repositories
{
    public abstract class RepositoryBase<TAggregateRoot, TPrimaryKey> : IRepository<TAggregateRoot, TPrimaryKey>
        where TAggregateRoot : class, IAggregateRoot<TPrimaryKey>
    {
        public abstract IQueryable<TAggregateRoot> GetAll();

        public abstract Task<TAggregateRoot> Get(TPrimaryKey id, CancellationToken cancellationToken = default);

        public abstract Task<TAggregateRoot> Get(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken = default);

        public abstract Task Insert(TAggregateRoot entity, CancellationToken cancellationToken = default);

        public abstract Task Update(TAggregateRoot entity, CancellationToken cancellationToken = default);

        public async Task Update(TPrimaryKey id, Func<TAggregateRoot, Task> updateAction, CancellationToken cancellationToken = default)
        {
            TAggregateRoot aggregate = await Get(id, cancellationToken);
            await updateAction(aggregate);
            await Update(aggregate, cancellationToken);
        }

        public abstract Task Delete(TAggregateRoot entity, CancellationToken cancellationToken = default);

        public async Task Delete(TPrimaryKey id, CancellationToken cancellationToken = default)
        {
            TAggregateRoot aggregate = await Get(id, cancellationToken);
            await Delete(aggregate, cancellationToken);
        }

        public async Task Delete(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken = default)
        {
            foreach (TAggregateRoot aggregate in GetAll().Where(predicate))
            {
                await Delete(aggregate, cancellationToken);
            }
        }
    }
}
