using System;

using DDDify.Tests.ProductContext.Aggregates.Events;

namespace DDDify.Tests.ProductContext.Aggregates
{
    public partial class Product
    {
        private void When(ProductCreated @event)
        {
            Id = Guid.NewGuid();
            Name = @event.Name;
        }

        private void When(ProductNameChanged @event)
        {
            Name = @event.Name;
        }
    }
}
