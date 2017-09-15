#if NET451 || CORE20
namespace Cql.Core.Messaging
{
    using System.Net.Mail;
    using System.Threading.Tasks;

    public class SystemNetMailSmtpClient
    {
        private readonly SmtpConfig _smtpConfig;

        public SystemNetMailSmtpClient(SmtpConfig smtpConfig)
        {
            this._smtpConfig = smtpConfig;
        }

        public Task SendMessage(IMessage message)
        {
            var mailMessage = EmailMessage.CreateFromMessage(message);

            var client = this.CreateClient();

            return client.SendMailAsync(mailMessage);
        }

        private SmtpClient CreateClient()
        {
            var client = new SmtpClient(this._smtpConfig.Host, this._smtpConfig.Port);

            if (this._smtpConfig.Credentials != null)
            {
                client.Credentials = this._smtpConfig.Credentials;
            }

            if (this._smtpConfig.EnableSsl.HasValue)
            {
                client.EnableSsl = this._smtpConfig.EnableSsl.Value;
            }

            return client;
        }
    }
}
#endif
