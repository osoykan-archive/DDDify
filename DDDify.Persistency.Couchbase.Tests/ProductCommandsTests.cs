using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class ProductCommandsTests : ApplicationTestBase
    { 
        [Fact]
        public async Task Create_Product()
        {
            Guid productId = Random<Guid>._;
            string name = Random<string>._;

            await The<IMediator>().Send(new CreateProductCommand(productId, name));

            Query<Product>(x => x.Id == productId).Should().NotBeNull();
        }
    }
}
