using System;

using DDDify.Bus;

namespace DDDify.Tests.ProductContext.Aggregates.Events
{
    public class ProductNameChanged : Event
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
