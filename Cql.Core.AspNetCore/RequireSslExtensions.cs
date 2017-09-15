using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Cql.Core.AspNetCore
{
    public static class RequireSslExtensions
    {
        public static IApplicationBuilder UseRequireSslRedirect(this IApplicationBuilder app, RequireSslOptions options = null)
        {
            if (options == null)
            {
                options = RequireSslOptions.Default();
            }

            return app.UseMiddleware<RequireSslMiddleware>(Options.Create(options));
        }
    }
}
