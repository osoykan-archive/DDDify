using System.Threading.Tasks;

namespace DDDify.Bus
{
    public interface IEventPublisher
    {
        Task Publish<T>(T @event) where T : Event;
    }
}
