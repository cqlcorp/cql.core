using System;
using System.IO;

namespace Cql.Core.Messaging
{
    public class MessageAttachment : IDisposable
    {
        private const string DefaultContentType = "application/octet-stream";
        private string _contentType;

        public string AttachmentId { get; set; } = Guid.NewGuid().ToString();

        public string ContentType
        {
            get { return string.IsNullOrEmpty(_contentType) ? DefaultContentType : _contentType; }
            set { _contentType = value; }
        }

        public string AttachmentName { get; set; }

        public string AttachmentPath { get; set; }

        public Stream ContentStream { get; set; }

        public AttachmentType Type
        {
            get
            {
                if (!string.IsNullOrEmpty(AttachmentPath))
                {
                    return AttachmentType.FilePath;
                }

                return ContentStream != null ? AttachmentType.Stream : AttachmentType.None;
            }
        }

#if NET451

        public System.Net.Mail.Attachment CreateAttachment()
        {
            switch (Type)
            {
                case AttachmentType.None:
                    return null;

                case AttachmentType.FilePath:
                    return CreateAttachmentFromFilePath(AttachmentPath);

                case AttachmentType.Stream:
                    return CreateAttachmentFromStream(ContentStream);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private System.Net.Mail.Attachment CreateAttachmentFromFilePath(string attachmentPath)
        {
            var attachment = new System.Net.Mail.Attachment(attachmentPath, new System.Net.Mime.ContentType(ContentType));
            var filepathContentDisposition = attachment.ContentDisposition;
            filepathContentDisposition.DispositionType = "attachment";
            filepathContentDisposition.CreationDate = File.GetCreationTime(attachmentPath);
            filepathContentDisposition.ModificationDate = File.GetLastWriteTime(attachmentPath);
            filepathContentDisposition.ReadDate = File.GetLastAccessTime(attachmentPath);
            return attachment;
        }

        private System.Net.Mail.Attachment CreateAttachmentFromStream(Stream contentStream)
        {
            if (contentStream.CanSeek)
            {
                contentStream.Seek(0, SeekOrigin.Begin);
            }
            return new System.Net.Mail.Attachment(contentStream, AttachmentName, ContentType);
        }

#endif

        #region IDisposable

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                ContentStream?.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}
