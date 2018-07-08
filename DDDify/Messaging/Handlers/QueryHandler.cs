using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace DDDify.Messaging.Handlers
{
    public abstract class QueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : Query<TResponse>, IRequest
    {
        public abstract Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken);
    }
}