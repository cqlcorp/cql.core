namespace Cql.Core.Messaging
{
    using System.Threading.Tasks;

    public interface IMessageDispatchService
    {
        Task<MessageDispatchResult> SendAsync(IMessage message);
    }
}
