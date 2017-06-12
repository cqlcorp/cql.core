using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Owin;

namespace Cql.Core.Owin.WebPack
{
    public class RedirectClientUrlsToIndexMiddleware : OwinMiddleware
    {
        private readonly OwinMiddleware _next;

        public RedirectClientUrlsToIndexMiddleware(OwinMiddleware next) : base(next)
        {
            _next = next;
        }

        /// <summary>
        /// Serves up the /index.html page for any extension-less, non-API routes
        /// </summary>
        public override async Task Invoke(IOwinContext context)
        {
            if (context.Request.IsGetMethod() && context.Request.ExpectsHtml())
            {
                var uri = new UriBuilder(context.Request.Uri);

                var path = uri.Path;

                if (!IsIgnoredPath(path) && string.IsNullOrEmpty(Path.GetExtension(path)))
                {
                    if (await WebPackConfiguration.SendDefaultResponseHandler(context, WebPackConfiguration.DefaultFilePath))
                    {
                        return;
                    }
                }
            }

            await _next.Invoke(context);
        }

        internal static async Task<bool> SendDefaultResponse(IOwinContext context, string defaultFilePath)
        {
            var response = context.Response;

            if (response.SupportsSendFile())
            {
                response.ContentType = "text/html";

                await response.SendFileAsync(defaultFilePath);
                return true;
            }

            return false;
        }

        protected virtual bool IsIgnoredPath(string path)
        {
            return path.StartsWith("/api");
        }
    }
}
