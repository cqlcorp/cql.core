namespace Cql.Core.Owin
{
    using global::Owin;

    public static class QueryAuthTokenMiddlewareExtensions
    {
        /// <summary>
        /// Allows users to pass the Authorization token in the [access_token] query string parameter (for Authorized links
        /// requests that cannot pass headers).
        /// </summary>
        public static void SupportAccessTokenInQueryString(this IAppBuilder app)
        {
            app.Use(typeof(QueryAuthTokenMiddleware));
        }
    }
}
