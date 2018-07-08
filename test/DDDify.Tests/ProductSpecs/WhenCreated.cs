using System;

using TestBase;
using TestBase.ProductContext.Aggregates;
using TestBase.ProductContext.Aggregates.Events;

using Xunit;

namespace DDDify.Tests.ProductSpecs
{
    public class WhenCreated : DomainSpecBase
    {
        [Fact]
        public void Product_Should_Be_Created()
        {
            Guid productId = Guid.NewGuid();
            var name = Random<string>();

            var @event = new ProductCreated(productId, name);

            new ScenarioFor<Product>(
                () => Product.Create(productId, name)
                )
                .When(product => { })
                .ThenAssert(@event);
        }
    }
}
