using System.Collections.Generic;

using Owin;

namespace Cql.Core.Owin.WebPack
{
    public static class RedirectClientUrlsToIndexMiddlewareExtensions
    {
        public static IConfigurableRedirectClientUrlsToIndexMiddleware UseDefaultClientRedirect(this IAppBuilder app)
        {
            app.Use(typeof(RedirectClientUrlsToIndexMiddleware));

            return new ConfigurableRedirectClientUrlsToIndexMiddleware();
        }

        public static void SetDefaultPath(this IConfigurableRedirectClientUrlsToIndexMiddleware config, string defaultPath)
        {
            WebPackConfiguration.DefaultFilePath = defaultPath;
        }

        public static void SetDevServerPorts(this IConfigurableRedirectClientUrlsToIndexMiddleware config, params int[] ports)
        {
            WebPackConfiguration.DevServerPorts = new List<int>(ports);
        }

        public static void AddDevServerPort(this IConfigurableRedirectClientUrlsToIndexMiddleware config,int port)
        {
            WebPackConfiguration.DevServerPorts.Add(port);
        }

        public static void OnSendDefaultFile(this IConfigurableRedirectClientUrlsToIndexMiddleware config, HandleDefaultReponseDelegate fileHandler)
        {
            WebPackConfiguration.SendDefaultResponseHandler = fileHandler;
        }
    }
}
