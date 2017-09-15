namespace Cql.Core.Web
{
    public class FileContent
    {
        public byte[] ContentBytes { get; set; }

        public ContentDelivery? ContentDelivery { get; set; }

        public string ContentType { get; set; }

        public string FileName { get; set; }

        public bool NotFound => this.ContentBytes == null || this.ContentBytes.Length == 0;
    }
}
