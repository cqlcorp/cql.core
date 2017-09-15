namespace Cql.Core.Messaging
{
    using System;
    using System.IO;
    using System.Net.Mail;
    using System.Net.Mime;

    public class MessageAttachment : IDisposable
    {
        private const string DefaultContentType = "application/octet-stream";

        private string _contentType;

        private bool _disposed;

        public string AttachmentId { get; set; } = Guid.NewGuid().ToString();

        public string AttachmentName { get; set; }

        public string AttachmentPath { get; set; }

        public Stream ContentStream { get; set; }

        public string ContentType
        {
            get => string.IsNullOrEmpty(this._contentType) ? DefaultContentType : this._contentType;
            set => this._contentType = value;
        }

        public AttachmentType Type
        {
            get
            {
                if (!string.IsNullOrEmpty(this.AttachmentPath))
                {
                    return AttachmentType.FilePath;
                }

                return this.ContentStream != null ? AttachmentType.Stream : AttachmentType.None;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }

            if (disposing)
            {
                this.ContentStream?.Dispose();
            }

            this._disposed = true;
        }

#if NET451 || CORE20

        public Attachment CreateAttachment()
        {
            switch (this.Type)
            {
                case AttachmentType.None:
                    return null;

                case AttachmentType.FilePath:
                    return this.CreateAttachmentFromFilePath(this.AttachmentPath);

                case AttachmentType.Stream:
                    return this.CreateAttachmentFromStream(this.ContentStream);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Attachment CreateAttachmentFromFilePath(string attachmentPath)
        {
            var attachment = new Attachment(attachmentPath, new ContentType(this.ContentType));
            var filepathContentDisposition = attachment.ContentDisposition;
            filepathContentDisposition.DispositionType = "attachment";
            filepathContentDisposition.CreationDate = File.GetCreationTime(attachmentPath);
            filepathContentDisposition.ModificationDate = File.GetLastWriteTime(attachmentPath);
            filepathContentDisposition.ReadDate = File.GetLastAccessTime(attachmentPath);
            return attachment;
        }

        private Attachment CreateAttachmentFromStream(Stream contentStream)
        {
            if (contentStream.CanSeek)
            {
                contentStream.Seek(0, SeekOrigin.Begin);
            }

            return new Attachment(contentStream, this.AttachmentName, this.ContentType);
        }

#endif
    }
}
