using System.Threading;
using System.Threading.Tasks;

using DDDify.Messaging.Handlers;

using TestBase.ProductContext.Aggregates.Events;

namespace TestBase.ProductContext.EventHandlers
{
    public class ProductNameChangedEventHandler : DomainEventHandler<ProductNameChanged>
    {
        public override Task Handle(ProductNameChanged notification, CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
