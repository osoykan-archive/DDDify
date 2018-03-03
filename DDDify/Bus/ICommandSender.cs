using System.Threading.Tasks;

namespace DDDify.Bus
{
    public interface ICommandSender
    {
        Task Send<T>(T command) where T : Command;
    }
}
