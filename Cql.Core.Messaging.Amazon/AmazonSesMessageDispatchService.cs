using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace Cql.Core.Messaging.Amazon
{
    public class AmazonSesMessageDispatchService : IMessageDispatchService
    {
        private readonly AmazonSesConfig _config;

        public AmazonSesMessageDispatchService(AmazonSesConfig config)
        {
            _config = config;
        }

        public async Task<MessageDispatchResult> SendAsync(IMessage message)
        {
            var mailMessage = EmailMessage.CreateFromMessage(message);

            var credentials = GetCredentials();

            var client = new AmazonSimpleEmailServiceClient(credentials);

            var request = new SendRawEmailRequest
            {
                RawMessage = new RawMessage
                {
                    Data = RawMailHelper.ConvertMailMessageToMemoryStream(mailMessage)
                }
            };

            var response = await client.SendRawEmailAsync(request);

            if (response.HttpStatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(response.MessageId))
            {
                return new MessageDispatchResult
                {
                    Succeeded = true,
                    MessageId = response.MessageId
                };
            }

            return new MessageDispatchResult
            {
                Succeeded = false,
                MessageId = response.MessageId,
                Errors = $"{response.HttpStatusCode}: {StringifyMetaData(response.ResponseMetadata)}"
            };
        }

        private static string StringifyMetaData(ResponseMetadata responseMetadata)
        {
            if (responseMetadata?.Metadata == null)
            {
                return "";
            }

            var builder = new StringBuilder();

            foreach (var item in responseMetadata.Metadata)
            {
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
            }

            return builder.ToString();
        }

        private AWSCredentials GetCredentials()
        {
            return new BasicAWSCredentials(_config.AccessKey, _config.SecretKey);
        }
    }
}
