using System.Threading.Tasks;

namespace Cql.Core.Messaging
{
    public class EmailService : IEmailService
    {
        private readonly EmailServiceConfig _config;
        private readonly IMessageDispatchService _productionDispatcher;
        private readonly IMessageDispatchService _testDispatchService;

        public EmailService(EmailServiceConfig config, IMessageDispatchService productionDispatcher, IMessageDispatchService testDispatchService)
        {
            _config = config;
            _productionDispatcher = productionDispatcher;
            _testDispatchService = testDispatchService;
        }

        public Task<MessageDispatchResult> SendAsync(EmailMessage message)
        {
            if (message == null)
            {
                return Task.FromResult(new MessageDispatchResult
                {
                    Succeeded = false,
                    Errors = "The message cannot be null."
                });
            }

            if (message.From == null)
            {
                message.From = _config.EmailFromAddress;
            }

            if (message.IsBodyHtml == null)
            {
                message.IsBodyHtml = HtmlUtils.IsHtml(message.Body);
            }

            if (!message.IsSendable)
            {
                return Task.FromResult(new MessageDispatchResult
                {
                    Succeeded = false,
                    Errors = "The message is not sendable."
                });
            }

            if (_config.DispatchMode == DispatchMode.Production || message.AllowSendingInTestEnvironment)
            {
                return _productionDispatcher.SendAsync(message);
            }

            return _testDispatchService.SendAsync(message);
        }
    }
}
