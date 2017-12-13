using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using DDDify.Aggregates;
using DDDify.Repositories;

using Microsoft.EntityFrameworkCore;

namespace DDDify.Tests.EfCore
{
    public class EfRepository<TDbContext, TAggregateRoot, TPrimarKey> :
        RepositoryBase<TAggregateRoot, TPrimarKey>
        where TAggregateRoot : class, IAggregateRoot<TPrimarKey>
        where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;

        public EfRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override IQueryable<TAggregateRoot> GetAll()
        {
            return _dbContext.Set<TAggregateRoot>();
        }

        public override Task<TAggregateRoot> Get(TPrimarKey id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _dbContext.FindAsync<TAggregateRoot>(id);
        }

        public override Task<TAggregateRoot> Get(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetAll().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public override Task Insert(TAggregateRoot entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _dbContext.AddAsync(entity, cancellationToken);
        }

        public override Task Update(TAggregateRoot entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            _dbContext.Update(entity);
            return Task.CompletedTask;
        }

        public override Task Delete(TAggregateRoot entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            _dbContext.Remove(entity);
            return Task.CompletedTask;
        }
    }
}
