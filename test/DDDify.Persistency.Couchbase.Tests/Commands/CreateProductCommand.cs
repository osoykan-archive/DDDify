using System;

using DDDify.Messaging;

namespace DDDify.Persistency.Couchbase.Tests.Commands
{
    public class CreateProductCommand : Command
    {
        public CreateProductCommand(Guid productId,string name)
        {
            ProductId = productId;
            Name = name;
        }

        public Guid ProductId { get; }

        public string Name { get; }
    }
}
