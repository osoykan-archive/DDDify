using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

using DDDify.Repositories;
using DDDify.Tests.EfCore;
using DDDify.Uow;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DDDify.Tests
{
    public class EfUnitOfWork<TDbContext> : UnitOfWorkBase where TDbContext : DbContext
    {
        private readonly Func<TDbContext> _dbFactory;
        private readonly Action<UnitOfWorkOptions> _optionsCreator;

        public EfUnitOfWork(Func<TDbContext> dbFactory, Action<UnitOfWorkOptions> optionsCreator)
        {
            _dbFactory = dbFactory;
            _optionsCreator = optionsCreator;
        }

        public override async Task For<TAggregateRoot, TPrimaryKey>(
            Func<IRepository<TAggregateRoot, TPrimaryKey>, Task> when,
            Action onCompleted,
            Action<Exception> onFailed,
            CancellationToken cancellationToken = default(CancellationToken))

        {
            var opts = new UnitOfWorkOptions();
            _optionsCreator(opts);

            TDbContext dbContext = _dbFactory();

            try
            {
                using (IDbContextTransaction trx = await dbContext.Database.BeginTransactionAsync((opts.IsolationLevel ?? IsolationLevel.ReadUncommitted).ToSystemDataIsolationLevel(), cancellationToken))
                {
                    Id = trx.TransactionId.ToString();

                    await when(new EfRepository<TDbContext, TAggregateRoot, TPrimaryKey>(_dbFactory()));

                    await dbContext.SaveChangesAsync(cancellationToken);

                    trx.Commit();
                }
            }
            catch (Exception exception)
            {
                onFailed(exception);
                throw;
            }

            onCompleted();
        }
    }
}
