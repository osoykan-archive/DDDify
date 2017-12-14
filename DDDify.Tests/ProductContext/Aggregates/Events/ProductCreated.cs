using DDDify.Bus;

namespace DDDify.Tests.ProductContext.Aggregates.Events
{
    public class ProductCreated : Event
    {
        public string Name;

        public ProductCreated(string name)
        {
            Name = name;
        }
    }
}
