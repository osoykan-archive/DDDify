using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TestBase;
using TestBase.ProductContext.Aggregates;
using Xunit;

namespace DDDify.Persistency.Couchbase.Tests
{
    [Collection("CouchbaseCollection")]
    public class CouchbaseRepositoryTests : ApplicationTestBase
    {
        public CouchbaseRepositoryTests(CouchbaseDockerFixture fixture)
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
        public async Task Delete_Should_Work()
        {
            var repository = The<ICouchbaseRepository<Product>>();
            var productId = Random<Guid>._;
            var name = Random<string>._;

            var product = Product.Create(productId, name);
            await repository.Insert(product, CancellationToken.None);

            var createdDoc = repository.Get().FirstOrDefault(x => x.Id == productId);
            await repository.Delete(createdDoc, CancellationToken.None);
        }

        [Fact]
        public void Generate_Document_Id_Should_Work()
        {
            var repository = The<ICouchbaseRepository<Product>>();
            var productId = Random<Guid>._;
            var name = Random<string>._;

            var product = Product.Create(productId, name);
            var documentId = repository.GenerateDocumentId(product);

            documentId.Should().Be($"{typeof(Product).Name}:{productId}");
        }

        [Fact]
        public async Task Insert_Should_Work()
        {
            var repository = The<ICouchbaseRepository<Product>>();

            var productId = Random<Guid>._;
            var name = Random<string>._;

            var product = Product.Create(productId, name);

            await repository.Insert(product, CancellationToken.None);

            Query<Product>(x => x.Id == productId).Should().NotBeNull();
        }

        [Fact]
        public async Task Update_Should_Work()
        {
            var repository = The<ICouchbaseRepository<Product>>();

            var productId = Random<Guid>._;
            var name = Random<string>._;

            var product = Product.Create(productId, name);
            await repository.Insert(product, CancellationToken.None);

            var createdDoc = repository.Get().FirstOrDefault(x => x.Id == productId);

            var newName = Random<string>._;
            createdDoc.ChangeName(newName);

            var updatedDoc = await repository.Update(createdDoc, CancellationToken.None);

            updatedDoc.Name.Should().Be(newName);
        }
    }
}