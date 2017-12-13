using System;

using DDDify.Aggregates;
using DDDify.Tests.ProductContext.Aggregates.Events;

namespace DDDify.Tests
{
    public partial class Product : AggregateRoot<Guid>
    {
        protected Product()
        {
            Register<ProductNameChanged>(When);
            Register<ProductCreated>(When);
        }

        public string Name { get; protected set; }

        public void ChangeName(string withName)
        {
            ApplyChange(
                new ProductNameChanged(withName, Id)
            );
        }

        public static Product Create(string name)
        {
            var product = new Product();
            product.ApplyChange(
                new ProductCreated(name)
            );
            return product;
        }
    }
}
