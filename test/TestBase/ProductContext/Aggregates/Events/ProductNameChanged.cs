using System;
using DDDify.Messaging;

namespace TestBase.ProductContext.Aggregates.Events
{
    public class ProductNameChanged : Event
    {
        public ProductNameChanged(Guid productId, string name)
        {
            Name = name;
            ProductId = productId;
        }

        public Guid ProductId { get; }

        public string Name { get; }
    }
}