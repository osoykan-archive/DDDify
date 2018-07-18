using System.Threading;
using System.Threading.Tasks;
using DDDify.Messaging.Handlers;
using TestBase.ProductContext.Aggregates.Events;

namespace TestBase.ProductContext.EventHandlers
{
    public class ProductCreatedEventHandler : DomainEventHandler<ProductCreated>
    {
        public override Task Handle(ProductCreated notification, CancellationToken cancellationToken) =>
            Task.CompletedTask;
    }
}