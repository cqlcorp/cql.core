namespace Cql.Core.Owin
{
    using System;

    using Microsoft.Owin;

    public static class OwinRequestExtensions
    {
        public static bool ExpectsHtml(this IOwinRequest request)
        {
            return request.Accept.ToLower().Contains("html");
        }

        public static bool IsDeleteMethod(this IOwinRequest request)
        {
            return string.Equals("DELETE", request.Method, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsGetMethod(this IOwinRequest request)
        {
            return string.Equals("GET", request.Method, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsPostMethod(this IOwinRequest request)
        {
            return string.Equals("POST", request.Method, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsPutMethod(this IOwinRequest request)
        {
            return string.Equals("PUT", request.Method, StringComparison.OrdinalIgnoreCase);
        }
    }
}
