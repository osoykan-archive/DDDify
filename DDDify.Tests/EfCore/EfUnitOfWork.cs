using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

using DDDify.Aggregates;
using DDDify.Bus;
using DDDify.Repositories;
using DDDify.Uow;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace DDDify.Tests.EfCore
{
    public class EfUnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
    {
        private readonly Func<TDbContext> _dbFactory;
        private readonly IEventPublisher _publisher;

        public EfUnitOfWork(Func<TDbContext> dbFactory, IEventPublisher publisher)
        {
            _dbFactory = dbFactory;
            _publisher = publisher;
        }

        public async Task For<TAggregateRoot, TPrimaryKey>(
            Func<IRepository<TAggregateRoot, TPrimaryKey>, Task> when,
            Action<UnitOfWorkOptions> optionsCreator = null,
            Action onCompleted = null,
            Action<Exception> onFailed = null,
            bool throwIfNeeded = false,
            CancellationToken cancellationToken = default)
            where TAggregateRoot : class, IAggregateRoot<TPrimaryKey>

        {
            var opts = new UnitOfWorkOptions();
            optionsCreator?.Invoke(opts);

            Func<Task> internalOnCompleted = () => Task.CompletedTask;
            try
            {
                using (TDbContext dbContext = _dbFactory())
                {
                    using (IDbContextTransaction trx = await dbContext.Database.BeginTransactionAsync((opts.IsolationLevel ?? IsolationLevel.ReadUncommitted).ToSystemDataIsolationLevel(), cancellationToken))
                    {
                        Id = trx.TransactionId.ToString();

                        await when(new EfRepository<TDbContext, TAggregateRoot, TPrimaryKey>(dbContext));

                        internalOnCompleted = PrepareCompleteAction(dbContext);

                        await dbContext.SaveChangesAsync(cancellationToken);

                        trx.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                onFailed?.Invoke(ex);

                if (throwIfNeeded)
                {
                    throw;
                }
            }

            await internalOnCompleted();

            onCompleted?.Invoke();
        }

        public string Id { get; private set; }

        private Func<Task> PrepareCompleteAction(TDbContext dbContext)
        {
            if (dbContext.ChangeTracker.HasChanges())
            {
                List<EntityEntry> changes = dbContext.ChangeTracker.Entries().ToList();
                IEnumerable<IAggregateChangeTracker> trackedChanges = changes.Where(x => x.Entity is IAggregateChangeTracker)
                                                                             .Select(x => (IAggregateChangeTracker)x.Entity);

                return async () =>
                {
                    List<Event> events = trackedChanges.SelectMany(x => x.GetChanges()).OfType<Event>().ToList();
                    foreach (Event @event in events)
                    {
                        await _publisher.Publish(@event);
                    }
                };
            }

            return async () => await Task.CompletedTask;
        }
    }
}
