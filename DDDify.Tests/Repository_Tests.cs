using System;
using System.Threading.Tasks;
using System.Transactions;

using FluentAssertions;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using Xunit;

namespace DDDify.Tests
{
    public class Repository_Tests
    {
        [Fact]
        public async Task should_work()
        {
            var conn = new SqliteConnection("Data Source=:memory:");
            DbContextOptions<ProductDbContext> opts = new DbContextOptionsBuilder<ProductDbContext>()
                .UseSqlite(conn)
                .Options;

            conn.Open();

            var dbContext = new ProductDbContext(opts);
            await dbContext.Database.EnsureCreatedAsync();

            await new EfUnitOfWork<ProductDbContext>(() => dbContext, options => { options.IsolationLevel = IsolationLevel.Unspecified; })
                .For<Product, Guid>(async repository =>
                    {
                        Product product = Product.Create("pant");
                        await repository.Insert(product);

                        Product aggregate = await repository.Get(product.Id);

                        aggregate.Should().NotBeNull();
                    },
                    () => { },
                    exception => { });
        }
    }
}
