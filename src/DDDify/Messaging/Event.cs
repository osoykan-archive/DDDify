using MediatR;

namespace DDDify.Messaging
{
    public abstract class Event : INotification
    {
        public string CausationId { get; set; }
    }
}