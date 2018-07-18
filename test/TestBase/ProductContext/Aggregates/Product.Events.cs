using TestBase.ProductContext.Aggregates.Events;

namespace TestBase.ProductContext.Aggregates
{
    public partial class Product
    {
        private void When(ProductCreated @event)
        {
            Id = @event.ProductId;
            Name = @event.Name;
        }

        private void When(ProductNameChanged @event)
        {
            Name = @event.Name;
        }
    }
}