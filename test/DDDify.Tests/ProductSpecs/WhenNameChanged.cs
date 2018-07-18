using System;
using TestBase;
using TestBase.ProductContext.Aggregates;
using TestBase.ProductContext.Aggregates.Events;
using Xunit;

namespace DDDify.Tests.ProductSpecs
{
    public class WhenNameChanged : DomainSpecBase
    {
        [Fact]
        public void Given_Product_Then_Name_Should_Be_Changed()
        {
            var productId = Guid.NewGuid();
            var name = Random<string>();

            var @event = new ProductNameChanged(productId, name);

            new ScenarioForExisting<Product>()
                .Given(() => Product.Create(productId, Random<string>()))
                .When(product => { product.ChangeName(name); })
                .ThenAssert(@event)
                .AlsoAssert(product => product.Name == name);
        }
    }
}