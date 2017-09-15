namespace Cql.Core.Owin.WebPack
{
    using System.Collections.Generic;

    using global::Owin;

    public static class RedirectClientUrlsToIndexMiddlewareExtensions
    {
        public static void AddDevServerPort(this IConfigurableRedirectClientUrlsToIndexMiddleware config, int port)
        {
            WebPackConfiguration.DevServerPorts.Add(port);
        }

        public static void OnSendDefaultFile(this IConfigurableRedirectClientUrlsToIndexMiddleware config, HandleDefaultReponseDelegate fileHandler)
        {
            WebPackConfiguration.SendDefaultResponseHandler = fileHandler;
        }

        public static void SetDefaultPath(this IConfigurableRedirectClientUrlsToIndexMiddleware config, string defaultPath)
        {
            WebPackConfiguration.DefaultFilePath = defaultPath;
        }

        public static void SetDevServerPorts(this IConfigurableRedirectClientUrlsToIndexMiddleware config, params int[] ports)
        {
            WebPackConfiguration.DevServerPorts = new List<int>(ports);
        }

        public static IConfigurableRedirectClientUrlsToIndexMiddleware UseDefaultClientRedirect(this IAppBuilder app)
        {
            app.Use(typeof(RedirectClientUrlsToIndexMiddleware));

            return new ConfigurableRedirectClientUrlsToIndexMiddleware();
        }
    }
}
