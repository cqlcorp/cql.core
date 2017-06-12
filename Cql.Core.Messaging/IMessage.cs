using System;
using System.Collections.Generic;

namespace Cql.Core.Messaging
{
    public interface IMessage : IDisposable
    {
        bool AllowSendingInTestEnvironment { get; set; }

        List<MessageAttachment> Attachments { get; set; }

        List<MessageAddress> Bcc { get; set; }

        string Body { get; set; }

        List<MessageAddress> Cc { get; set; }

        MessageAddress From { get; set; }

        Guid Guid { get; set; }

        bool HasAttachments { get; }

        bool? IsBodyHtml { get; set; }

        bool IsSendable { get; }

        List<MessageAddress> ReplyTo { get; set; }

        string Subject { get; set; }

        List<MessageAddress> To { get; set; }
    }
}
