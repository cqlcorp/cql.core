// ***********************************************************************
// Assembly         : Cql.Core.Messaging
// Author           : jeremy.bell
// Created          : 09-14-2017
//
// Last Modified By : jeremy.bell
// Last Modified On : 09-14-2017
// ***********************************************************************
// <copyright file="EmailMessage.cs" company="CQL;Jeremy Bell">
//     2017 Cql Incorporated
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Cql.Core.Messaging
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mail;
    using System.Threading;

    /// <summary>
    /// Class EmailMessage.
    /// </summary>
    /// <seealso cref="Cql.Core.Messaging.IMessage" />
    public class EmailMessage : IMessage
    {
        /// <summary>
        /// The attachments
        /// </summary>
        private List<MessageAttachment> _attachments;

        /// <summary>
        /// The BCC
        /// </summary>
        private List<MessageAddress> _bcc;

        /// <summary>
        /// The cc
        /// </summary>
        private List<MessageAddress> _cc;

        /// <summary>
        /// The reply to
        /// </summary>
        private List<MessageAddress> _replyTo;

        /// <summary>
        /// To
        /// </summary>
        private List<MessageAddress> _to;

        /// <summary>
        /// Gets or sets a value indicating whether [allow sending in test environment].
        /// </summary>
        /// <value><c>true</c> if [allow sending in test environment]; otherwise, <c>false</c>.</value>
        public bool AllowSendingInTestEnvironment { get; set; }

        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        /// <value>The attachments.</value>
        public List<MessageAttachment> Attachments
        {
            get => LazyInitializer.EnsureInitialized(ref this._attachments);
            set => this._attachments = value;
        }

        /// <summary>
        /// Gets or sets the BCC.
        /// </summary>
        /// <value>The BCC.</value>
        public List<MessageAddress> Bcc
        {
            get => LazyInitializer.EnsureInitialized(ref this._bcc, () => new List<MessageAddress>());

            set => this._bcc = value;
        }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>The body.</value>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the cc.
        /// </summary>
        /// <value>The cc.</value>
        public List<MessageAddress> Cc
        {
            get => LazyInitializer.EnsureInitialized(ref this._cc, () => new List<MessageAddress>());

            set => this._cc = value;
        }

        /// <summary>
        /// Gets or sets from address.
        /// </summary>
        /// <value>A valid email address.</value>
        public MessageAddress From { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        public Guid Guid { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets a value indicating whether this instance has attachments.
        /// </summary>
        /// <value><c>true</c> if this instance has attachments; otherwise, <c>false</c>.</value>
        public bool HasAttachments => this._attachments != null && this._attachments.Count > 0;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is body HTML.
        /// </summary>
        /// <value><c>null</c> if [is body HTML] contains no value, <c>true</c> if [is body HTML]; otherwise, <c>false</c>.</value>
        public bool? IsBodyHtml { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is sendable.
        /// </summary>
        /// <value><c>true</c> if this instance is sendable; otherwise, <c>false</c>.</value>
        public bool IsSendable => !string.IsNullOrWhiteSpace(this.Body) && !string.IsNullOrWhiteSpace(this.Subject) && this.To.Count > 0 && this.From != null;

        /// <summary>
        /// Gets or sets the reply to.
        /// </summary>
        /// <value>The reply to.</value>
        public List<MessageAddress> ReplyTo
        {
            get
            {
                return LazyInitializer.EnsureInitialized(ref this._replyTo, () => new List<MessageAddress>());
            }

            set => this._replyTo = value;
        }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>The subject.</value>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>A collection of To email addresses.</value>
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

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
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
