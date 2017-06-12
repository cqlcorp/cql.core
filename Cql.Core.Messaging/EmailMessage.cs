using System;
using System.Collections.Generic;
using System.Threading;

namespace Cql.Core.Messaging
{
    public class EmailMessage : IMessage, IDisposable
    {
        private List<MessageAttachment> _attachments;
        private List<MessageAddress> _bcc;
        private List<MessageAddress> _cc;
        private List<MessageAddress> _replyTo;
        private List<MessageAddress> _to;

        public bool AllowSendingInTestEnvironment { get; set; }

        public List<MessageAttachment> Attachments
        {
            get { return LazyInitializer.EnsureInitialized(ref _attachments); }
            set { _attachments = value; }
        }

        public List<MessageAddress> Bcc
        {
            get { return LazyInitializer.EnsureInitialized(ref _bcc, () => new List<MessageAddress>()); }
            set { _bcc = value; }
        }

        public string Body { get; set; }

        public List<MessageAddress> Cc
        {
            get { return LazyInitializer.EnsureInitialized(ref _cc, () => new List<MessageAddress>()); }
            set { _cc = value; }
        }

        public MessageAddress From { get; set; }

        public Guid Guid { get; set; } = Guid.NewGuid();

        public bool HasAttachments => _attachments != null && _attachments.Count > 0;

        public bool? IsBodyHtml { get; set; }

        public bool IsSendable => !string.IsNullOrWhiteSpace(Body) && !string.IsNullOrWhiteSpace(Subject) && To.Count > 0 && From != null;

        public List<MessageAddress> ReplyTo
        {
            get { return LazyInitializer.EnsureInitialized(ref _replyTo, () => new List<MessageAddress>()); }
            set { _replyTo = value; }
        }

        public string Subject { get; set; }

        public List<MessageAddress> To
        {
            get { return LazyInitializer.EnsureInitialized(ref _to, () => new List<MessageAddress>()); }
            set { _to = value; }
        }

        public void Dispose()
        {
            if (!HasAttachments)
            {
                return;
            }

            foreach (var messageAttachment in Attachments)
            {
                messageAttachment?.Dispose();
            }
        }

#if NET451

        public static System.Net.Mail.MailMessage CreateFromMessage(IMessage message)
        {
            var msg = new System.Net.Mail.MailMessage
            {
                From = message.From,
                IsBodyHtml = message.IsBodyHtml.GetValueOrDefault(),
                Body = message.Body,
                Subject = message.Subject
            };


            foreach (var address in message.To)
            {
                msg.To.Add(address);
            }

            foreach (var address in message.Cc)
            {
                msg.CC.Add(address);
            }

            foreach (var address in message.Bcc)
            {
                msg.Bcc.Add(address);
            }

            foreach (var address in message.ReplyTo)
            {
                msg.ReplyToList.Add(address);
            }

            if (message.HasAttachments)
            {
                foreach (var attachment in message.Attachments)
                {
                    msg.Attachments.Add(attachment.CreateAttachment());
                }
            }

            return msg;
        }
#endif
    }
}
