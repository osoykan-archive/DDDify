using System.Threading.Tasks;

using DDDify.Bus;
using DDDify.Tests.ProductContext.Aggregates.Events;

namespace DDDify.Tests.ProductContext.EventHandlers
{
    public class DomainEventHandlers :
        IHandles<ProductCreated>,
        IHandles<ProductNameChanged>
    {
        public Task Handle(ProductCreated message) => Task.FromResult(0);

        public Task Handle(ProductNameChanged message) => Task.CompletedTask;
    }
}
