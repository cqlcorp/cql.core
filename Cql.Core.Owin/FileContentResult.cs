using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

using Cql.Core.Web;

namespace Cql.Core.Owin
{
    public class FileContentResult : IHttpActionResult
    {
        private readonly byte[] _content;
        private readonly ContentDelivery _contentDelivery;
        private readonly string _contentType;
        private readonly string _fileName;

        public FileContentResult(FileContent content)
        {
            _content = content.ContentBytes;
            _contentType = content.ContentType;
            _contentDelivery = content.ContentDelivery.GetValueOrDefault(ContentDelivery.Inline);
            _fileName = content.FileName;
        }

        public FileContentResult(byte[] content, string contentType, ContentDelivery contentDelivery = ContentDelivery.Inline, string fileName = null)
        {
            _content = content;
            _contentType = contentType;
            _contentDelivery = contentDelivery;
            _fileName = fileName;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            if (_content == null || _content.Length == 0L)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(_content)
            };

            var headers = response.Content.Headers;

            headers.ContentType = new MediaTypeHeaderValue(_contentType);

            SetContentDispositionHeader(headers);

            return Task.FromResult(response);
        }

        private void SetContentDispositionHeader(HttpContentHeaders headers)
        {
            if (_contentType != "application/pdf")
            {
                return;
            }

            var dispositionType = _contentDelivery == ContentDelivery.Inline ? "inline" : "attachment";

            var contentDisposition = new ContentDispositionHeaderValue(dispositionType);

            if (!string.IsNullOrWhiteSpace(_fileName))
            {
                var fileName = $"{SlugUtility.GenerateSlug(Path.GetFileNameWithoutExtension(_fileName))}{Path.GetExtension(_fileName)}";

                contentDisposition.FileName = fileName;
            }

            headers.ContentDisposition = contentDisposition;
        }
    }
}
