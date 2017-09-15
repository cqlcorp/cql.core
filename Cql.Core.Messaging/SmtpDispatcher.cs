namespace Cql.Core.Messaging
{
    using System;
    using System.Threading.Tasks;

    public class SmtpDispatcher : IMessageDispatchService
    {
        private readonly SmtpConfig _smtpConfig;

        public SmtpDispatcher(SmtpConfig smtpConfig)
        {
            this._smtpConfig = smtpConfig;
        }

        public async Task<MessageDispatchResult> SendAsync(IMessage message)
        {
            try
            {
                var messageId = Guid.NewGuid().ToString("n");

#if COREFX && !CORE20
                var smptClient = new MailKitSmtpClient(_smtpConfig);
                await smptClient.SendMessage(message);
#else
                var smptClient = new SystemNetMailSmtpClient(this._smtpConfig);
                await smptClient.SendMessage(message);
#endif
                return new MessageDispatchResult { Succeeded = true, MessageId = messageId };
            }
            catch (Exception ex)
            {
                return new MessageDispatchResult { Succeeded = false, Errors = ex.Message };
            }
            finally
            {
                message?.Dispose();
            }
        }
    }
}
