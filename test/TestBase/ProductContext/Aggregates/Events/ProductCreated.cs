using System;
using DDDify.Messaging;

namespace TestBase.ProductContext.Aggregates.Events
{
    public class ProductCreated : Event
    {
        public ProductCreated(Guid productId, string name)
        {
            ProductId = productId;
            Name = name;
        }

        public Guid ProductId { get; }

        public string Name { get; }
    }
}