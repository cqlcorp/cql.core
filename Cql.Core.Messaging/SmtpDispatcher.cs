using System;
using System.Threading.Tasks;

namespace Cql.Core.Messaging
{
    public class SmtpDispatcher : IMessageDispatchService
    {
        private readonly SmtpConfig _smtpConfig;

        public SmtpDispatcher(SmtpConfig smtpConfig)
        {
            _smtpConfig = smtpConfig;
        }

        public async Task<MessageDispatchResult> SendAsync(IMessage message)
        {
            try
            {
                var messageId = Guid.NewGuid().ToString("n");

#if COREFX
                var smptClient = new MailKitSmtpClient(_smtpConfig);
                await smptClient.SendMessage(message);
#else
                var smptClient = new SystemNetMailSmtpClient(_smtpConfig);
                await smptClient.SendMessage(message);
#endif
                return new MessageDispatchResult
                {
                    Succeeded = true,
                    MessageId = messageId
                };
            }
            catch (Exception ex)
            {
                return new MessageDispatchResult
                {
                    Succeeded = false,
                    Errors = ex.Message
                };
            }
            finally
            {
                message?.Dispose();
            }
        }
    }
}
