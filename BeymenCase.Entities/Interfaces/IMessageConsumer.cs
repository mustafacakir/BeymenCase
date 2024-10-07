using System.Threading.Tasks;

namespace BeymenCase.Core.Interfaces
{
    public interface IMessageConsumer
    {
        Task<string> ReceiveMessageAsync();
    }
}
