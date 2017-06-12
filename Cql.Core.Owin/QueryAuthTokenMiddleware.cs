using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Owin;

namespace Cql.Core.Owin
{
    public class QueryAuthTokenMiddleware : OwinMiddleware
    {
        private readonly OwinMiddleware _next;

        public QueryAuthTokenMiddleware(OwinMiddleware next) : base(next)
        {
            _next = next;
        }

        public override Task Invoke(IOwinContext context)
        {
            if (!context.Request.QueryString.HasValue || !string.IsNullOrWhiteSpace(context.Request.Headers.Get("Authorization")))
            {
                return _next.Invoke(context);
            }

            var queryString = context.Request.Uri.ParseQueryString();

            string token = queryString.Get("access_token");

            if (!string.IsNullOrWhiteSpace(token))
            {
                context.Request.Headers.Add("Authorization", new[] {$"Bearer {token}"});
            }

            return _next.Invoke(context);
        }
    }
}
