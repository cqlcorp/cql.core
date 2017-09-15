namespace Cql.Core.Messaging
{
    using System.Threading.Tasks;

    public class EmailService : IEmailService
    {
        private readonly EmailServiceConfig _config;

        private readonly IMessageDispatchService _productionDispatcher;

        private readonly IMessageDispatchService _testDispatchService;

        public EmailService(EmailServiceConfig config, IMessageDispatchService productionDispatcher, IMessageDispatchService testDispatchService)
        {
            this._config = config;
            this._productionDispatcher = productionDispatcher;
            this._testDispatchService = testDispatchService;
        }

        public Task<MessageDispatchResult> SendAsync(EmailMessage message)
        {
            if (message == null)
            {
                return Task.FromResult(new MessageDispatchResult { Succeeded = false, Errors = "The message cannot be null." });
            }

            if (message.From == null)
            {
                message.From = this._config.EmailFromAddress;
            }

            if (message.IsBodyHtml == null)
            {
                message.IsBodyHtml = HtmlUtils.IsHtml(message.Body);
            }

            if (!message.IsSendable)
            {
                return Task.FromResult(new MessageDispatchResult { Succeeded = false, Errors = "The message is not sendable." });
            }

            if (this._config.DispatchMode == DispatchMode.Production || message.AllowSendingInTestEnvironment)
            {
                return this._productionDispatcher.SendAsync(message);
            }

            return this._testDispatchService.SendAsync(message);
        }
    }
}
