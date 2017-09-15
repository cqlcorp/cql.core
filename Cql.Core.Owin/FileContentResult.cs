namespace Cql.Core.Owin
{
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Cql.Core.Web;

    public class FileContentResult : IHttpActionResult
    {
        private readonly byte[] _content;

        private readonly ContentDelivery _contentDelivery;

        private readonly string _contentType;

        private readonly string _fileName;

        public FileContentResult(FileContent content)
        {
            this._content = content.ContentBytes;
            this._contentType = content.ContentType;
            this._contentDelivery = content.ContentDelivery.GetValueOrDefault(ContentDelivery.Inline);
            this._fileName = content.FileName;
        }

        public FileContentResult(byte[] content, string contentType, ContentDelivery contentDelivery = ContentDelivery.Inline, string fileName = null)
        {
            this._content = content;
            this._contentType = contentType;
            this._contentDelivery = contentDelivery;
            this._fileName = fileName;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            if (this._content == null || this._content.Length == 0L)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(this._content) };

            var headers = response.Content.Headers;

            headers.ContentType = new MediaTypeHeaderValue(this._contentType);

            this.SetContentDispositionHeader(headers);

            return Task.FromResult(response);
        }

        private void SetContentDispositionHeader(HttpContentHeaders headers)
        {
            if (this._contentType != "application/pdf")
            {
                return;
            }

            var dispositionType = this._contentDelivery == ContentDelivery.Inline ? "inline" : "attachment";

            var contentDisposition = new ContentDispositionHeaderValue(dispositionType);

            if (!string.IsNullOrWhiteSpace(this._fileName))
            {
                var fileName = $"{SlugUtility.GenerateSlug(Path.GetFileNameWithoutExtension(this._fileName))}{Path.GetExtension(this._fileName)}";

                contentDisposition.FileName = fileName;
            }

            headers.ContentDisposition = contentDisposition;
        }
    }
}
