using System;
using System.Data.Common;
using System.Threading.Tasks;

using DDDify.Tests.EfCore;
using DDDify.Tests.ProductContext;
using DDDify.Tests.ProductContext.Aggregates;
using DDDify.Tests.ProductContext.Aggregates.Events;
using DDDify.Tests.ProductContext.EventHandlers;

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

            var bus = new InMemoryBus();
            var handler = new DomainEventHandlers();
            await bus.Register<ProductCreated>(async @event => await handler.Handle(@event));
            await bus.Register<ProductNameChanged>(async @event => await handler.Handle(@event));

            var dbContext = new ProductDbContext(opts);
            await dbContext.Database.EnsureCreatedAsync();

            await new EfUnitOfWork<ProductDbContext>(() => dbContext, bus)
                .For<Product, Guid>(async repository =>
                {
                    Product product = Product.Create("pant");
                    await repository.Insert(product);

                    Product aggregate = await repository.Get(product.Id);

                    aggregate.Should().NotBeNull();
                });
        }

        [Fact]
        public async Task ParallelExecution_should_work_as_thread_safe()
        {
            var conn = new SqliteConnection("Data Source=:memory:");

            DbContextOptions OptsFactory(DbConnection connection)
            {
                DbContextOptions opts = new DbContextOptionsBuilder().UseSqlite(connection).Options;
                connection.Open();
                return opts;
            }

            var bus = new InMemoryBus();
            var handler = new DomainEventHandlers();
            await bus.Register<ProductCreated>(async @event => await handler.Handle(@event));
            await bus.Register<ProductNameChanged>(async @event => await handler.Handle(@event));

            ProductDbContext DBFactory()
            {
                var db = new ProductDbContext(OptsFactory(conn));
                db.Database.EnsureCreated();
                return db;
            }

            await new EfUnitOfWork<ProductDbContext>(DBFactory, bus)
                .For<Product, Guid>(async repository =>
                    {
                        Product product = Product.Create($"pant1");
                        await repository.Insert(product);
                    },
                    throwIfNeeded: true);

            await new EfUnitOfWork<ProductDbContext>(DBFactory, bus)
                .For<Product, Guid>(async repository =>
                {
                    int count = await repository.GetAll().CountAsync();

                    count.Should().Be(1);

                    Product product = await repository.Get(x => x.Name == "pant1");
                    product.ChangeName("anan");
                }, throwIfNeeded: true);
        }
    }
}
