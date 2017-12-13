using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DDDify.Aggregates;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DDDify.Tests
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            List<EntityEntry> changes = ChangeTracker.Entries().ToList();

            IEnumerable<IAggregateChangeTracker> trackedChanges = changes.Where(x => x.Entity is IAggregateChangeTracker).Select(x => (IAggregateChangeTracker)x.Entity);

            foreach (IAggregateChangeTracker aggregate in trackedChanges)
            {
                List<object> events = aggregate.GetChanges().ToList();

                events.ForEach(@event =>
                {
                    //publish...
                });
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
