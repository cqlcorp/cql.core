#if COREFX && !CORE20
using System;
using System.Threading.Tasks;

using MailKit.Net.Smtp;

using MimeKit;

namespace Cql.Core.Messaging
{
    public class MailKitSmtpClient
    {
        private readonly SmtpConfig _smtpConfig;

        public MailKitSmtpClient(SmtpConfig smtpConfig)
        {
            _smtpConfig = smtpConfig;
        }

        public async Task<string> SendMessage(IMessage message)
        {
            var emailMessage = CreateEmailMessage(message);

            using (var client = new SmtpClient())
            {
                if (_smtpConfig.IgnoreCertificateErrors)
                {
                    client.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                }

                try
                {
                    await client.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, _smtpConfig.EnableSsl.GetValueOrDefault()).ConfigureAwait(false);

                    if (!string.IsNullOrEmpty(_smtpConfig.Credentials?.Password))
                    {
                        await client.AuthenticateAsync(_smtpConfig.Credentials).ConfigureAwait(false);
                    }

                    await client.SendAsync(emailMessage).ConfigureAwait(false);
                }
                finally
                {
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }

                return emailMessage.MessageId;
            }
        }

        private static MimeMessage CreateEmailMessage(IMessage message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(message.From.AsMailboxAddress());

            foreach (var messageAddress in message.To)
            {
                emailMessage.To.Add(messageAddress.AsMailboxAddress());
            }

            foreach (var messageAddress in message.Cc)
            {
                emailMessage.Cc.Add(messageAddress.AsMailboxAddress());
            }

            foreach (var messageAddress in message.Bcc)
            {
                emailMessage.Bcc.Add(messageAddress.AsMailboxAddress());
            }

            foreach (var messageAddress in message.ReplyTo)
            {
                emailMessage.ReplyTo.Add(messageAddress.AsMailboxAddress());
            }

            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder();

            if (message.IsBodyHtml.GetValueOrDefault())
            {
                bodyBuilder.HtmlBody = message.Body;
            }
            else
            {
                bodyBuilder.TextBody = message.Body;
            }

            if (message.HasAttachments)
            {

                foreach (var attachment in message.Attachments)
                {
                    var contentType = string.IsNullOrEmpty(attachment.ContentType) ? default(ContentType) : ContentType.Parse(attachment.ContentType);

                    switch (attachment.Type)
                    {
                        case AttachmentType.None:
                            break;

                        case AttachmentType.FilePath:
                            bodyBuilder.Attachments.Add(attachment.AttachmentPath, contentType);
                            break;

                        case AttachmentType.Stream:
                            bodyBuilder.Attachments.Add(attachment.AttachmentName, attachment.ContentStream, contentType);
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }
    }

    public static class ExtensionsForMailKit
    {
        public static MailboxAddress AsMailboxAddress(this IMessageAddress address)
        {
            return new MailboxAddress(address.DisplayName, address.Address);
        }
    }
}
#endif
