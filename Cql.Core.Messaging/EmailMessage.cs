namespace Cql.Core.Messaging
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mail;
    using System.Threading;

    public class EmailMessage : IMessage
    {
        private List<MessageAttachment> _attachments;

        private List<MessageAddress> _bcc;

        private List<MessageAddress> _cc;

        private List<MessageAddress> _replyTo;

        private List<MessageAddress> _to;

        public bool AllowSendingInTestEnvironment { get; set; }

        public List<MessageAttachment> Attachments
        {
            get => LazyInitializer.EnsureInitialized(ref this._attachments);
            set => this._attachments = value;
        }

        public List<MessageAddress> Bcc
        {
            get
            {
                return LazyInitializer.EnsureInitialized(ref this._bcc, () => new List<MessageAddress>());
            }

            set => this._bcc = value;
        }

        public string Body { get; set; }

        public List<MessageAddress> Cc
        {
            get
            {
                return LazyInitializer.EnsureInitialized(ref this._cc, () => new List<MessageAddress>());
            }

            set => this._cc = value;
        }

        public MessageAddress From { get; set; }

        public Guid Guid { get; set; } = Guid.NewGuid();

        public bool HasAttachments => this._attachments != null && this._attachments.Count > 0;

        public bool? IsBodyHtml { get; set; }

        public bool IsSendable => !string.IsNullOrWhiteSpace(this.Body) && !string.IsNullOrWhiteSpace(this.Subject) && this.To.Count > 0 && this.From != null;

        public List<MessageAddress> ReplyTo
        {
            get
            {
                return LazyInitializer.EnsureInitialized(ref this._replyTo, () => new List<MessageAddress>());
            }

            set => this._replyTo = value;
        }

        public string Subject { get; set; }

        public List<MessageAddress> To
        {
            get
            {
                return LazyInitializer.EnsureInitialized(ref this._to, () => new List<MessageAddress>());
            }

            set => this._to = value;
        }

#if NET451 || CORE20

        public static MailMessage CreateFromMessage(IMessage message)
        {
            var msg = new MailMessage { From = message.From, IsBodyHtml = message.IsBodyHtml.GetValueOrDefault(), Body = message.Body, Subject = message.Subject };

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

        public void Dispose()
        {
            if (!this.HasAttachments)
            {
                return;
            }

            foreach (var messageAttachment in this.Attachments)
            {
                messageAttachment?.Dispose();
            }
        }
    }
}
