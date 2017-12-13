using System;

namespace DDDify.Tests.ProductContext.Aggregates.Events
{
    public class ProductNameChanged
    {
        public Guid Id;
        public string Name;

        public ProductNameChanged(string name, Guid id)
        {
            Name = name;
            Id = id;
        }
    }
}