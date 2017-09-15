using Cql.Core.Web;

using Microsoft.AspNetCore.Http;

namespace Cql.Core.AspNetCore.Utils
{
    public static class MobileDeviceExtensions
    {
        public static bool IsMobileRequest(this HttpContext httpContext)
        {
            return MobileDeviceDetection.IsMobileBrowser(httpContext.Request.UserAgent());
        }
    }
}
