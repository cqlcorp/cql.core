using System.Threading.Tasks;

namespace Cql.Core.Messaging
{
    public interface IEmailService
    {
        Task<MessageDispatchResult> SendAsync(EmailMessage message);
    }
}
