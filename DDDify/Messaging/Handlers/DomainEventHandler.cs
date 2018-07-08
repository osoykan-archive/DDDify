using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace DDDify.Messaging.Handlers
{
    public abstract class DomainEventHandler<TEvent> : INotificationHandler<TEvent>
        where TEvent : Event
    {
        public abstract Task Handle(TEvent notification, CancellationToken cancellationToken);
    }
}