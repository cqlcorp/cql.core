namespace Cql.Core.Web
{
    public class FileContent
    {
        public ContentDelivery? ContentDelivery { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public bool NotFound => ContentBytes == null || ContentBytes.Length == 0;

        public byte[] ContentBytes { get; set; }
    }
}
