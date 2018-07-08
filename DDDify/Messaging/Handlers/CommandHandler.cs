using System.Threading;
using System.Threading.Tasks;

using MediatR;

namespace DDDify.Messaging.Handlers
{
    public abstract class CommandHandler<TCommand> : AsyncRequestHandler<TCommand>
        where TCommand : Command
    {
        protected abstract override Task Handle(TCommand command, CancellationToken cancellationToken);
    }
}