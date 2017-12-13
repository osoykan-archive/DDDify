using DDDify.Tests.ProductContext.Aggregates.Events;

namespace DDDify.Tests
{
    public partial class Product
    {
        private void When(ProductCreated @event)
        {
            Name = @event.Name;
        }

        private void When(ProductNameChanged @event)
        {
            Name = @event.Name;
        }
    }
}
