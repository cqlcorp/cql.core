namespace Cql.Core.Owin
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.Owin;

    public class QueryAuthTokenMiddleware : OwinMiddleware
    {
        private readonly OwinMiddleware _next;

        public QueryAuthTokenMiddleware(OwinMiddleware next)
            : base(next)
        {
            this._next = next;
        }

        public override Task Invoke(IOwinContext context)
        {
            if (!context.Request.QueryString.HasValue || !string.IsNullOrWhiteSpace(context.Request.Headers.Get("Authorization")))
            {
                return this._next.Invoke(context);
            }

            var queryString = context.Request.Uri.ParseQueryString();

            var token = queryString.Get("access_token");

            if (!string.IsNullOrWhiteSpace(token))
            {
                context.Request.Headers.Add("Authorization", new[] { $"Bearer {token}" });
            }

            return this._next.Invoke(context);
        }
    }
}
