using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Cql.Core.AspNetCore
{
    public class RequireSslMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RequireSslOptions _options;

        public RequireSslMiddleware(RequestDelegate next, IOptions<RequireSslOptions> options)
        {
            _next = next;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;

            if (request.IsHttps)
            {
                await _next(context);
                return;
            }

            if (string.Equals(request.Method, "GET", StringComparison.OrdinalIgnoreCase))
            {
                var host = _options.SslPort != 443
                               ? new HostString(request.Host.Host, _options.SslPort)
                               : new HostString(request.Host.Host);

                string newUrl = $"https://{host}{request.PathBase}{request.Path}{request.QueryString}";

                context.Response.Redirect(newUrl, true);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("This site requires HTTPS.");
            }
        }
    }
}
