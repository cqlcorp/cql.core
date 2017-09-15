namespace Cql.Core.Messaging
{
    using System.Threading.Tasks;

    public interface IEmailService
    {
        Task<MessageDispatchResult> SendAsync(EmailMessage message);
    }
}
