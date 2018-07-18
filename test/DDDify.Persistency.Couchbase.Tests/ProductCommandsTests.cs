using System;
using System.Threading.Tasks;
using DDDify.Persistency.Couchbase.Tests.Commands;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TestBase;
using TestBase.ProductContext.Aggregates;
using Xunit;

namespace DDDify.Persistency.Couchbase.Tests
{
    [Collection("CouchbaseCollection")]
    public class ProductCommandsTests : ApplicationTestBase
    {
        public ProductCommandsTests(CouchbaseDockerFixture fixture)
        {
            _fixture = fixture;

            Building(services =>
            {
                services.AddDDDify(builder =>
                {
                    builder.UseCouchbase(Configuration)
                        .AddBucket<Product>("ProductContext");
                });
                services.AddLogging();
            }).Ok();
        }

        private CouchbaseDockerFixture _fixture;

        [Fact]
        public async Task Create_Product()
        {
            var productId = Random<Guid>._;
            var name = Random<string>._;

            await The<IMediator>().Send(new CreateProductCommand(productId, name));

            Query<Product>(x => x.Id == productId).Should().NotBeNull();
        }
    }
}