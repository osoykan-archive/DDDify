using MediatR;

namespace DDDify.Messaging
{
    public abstract class Query<TReponse> : IRequest<TReponse>
    {
    }
}