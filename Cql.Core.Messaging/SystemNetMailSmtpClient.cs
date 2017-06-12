#if NET451
using System.Net.Mail;
using System.Threading.Tasks;

namespace Cql.Core.Messaging
{
    public class SystemNetMailSmtpClient
    {
        private readonly SmtpConfig _smtpConfig;

        public SystemNetMailSmtpClient(SmtpConfig smtpConfig)
        {
            _smtpConfig = smtpConfig;
        }

        public Task SendMessage(IMessage message)
        {
            var mailMessage = EmailMessage.CreateFromMessage(message);

            var client = CreateClient();

            return client.SendMailAsync(mailMessage);
        }

        private SmtpClient CreateClient()
        {
            var client = new SmtpClient(_smtpConfig.Host, _smtpConfig.Port);

            if (_smtpConfig.Credentials != null)
            {
                client.Credentials = _smtpConfig.Credentials;
            }

            if (_smtpConfig.EnableSsl.HasValue)
            {
                client.EnableSsl = _smtpConfig.EnableSsl.Value;
            }

            return client;
        }
    }
}
#endif
