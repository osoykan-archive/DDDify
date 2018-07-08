using MediatR;

namespace DDDify.Messaging
{
    public abstract class Command : IRequest
    {
        public string CorrelationId { get; set; }
    }
}