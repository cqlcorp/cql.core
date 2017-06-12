using System.Threading.Tasks;

namespace Cql.Core.Messaging
{
    public interface IMessageDispatchService
    {
        Task<MessageDispatchResult> SendAsync(IMessage message);
    }
}
