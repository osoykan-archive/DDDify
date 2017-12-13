namespace DDDify.Tests.ProductContext.Aggregates.Events
{
    public class ProductCreated
    {
        public string Name;

        public ProductCreated(string name)
        {
            Name = name;
        }
    }
}