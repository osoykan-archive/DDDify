using System.Threading.Tasks;

namespace DDDify.Bus
{
    public interface IHandles<T>
    {
        Task Handle(T message);
    }
}
