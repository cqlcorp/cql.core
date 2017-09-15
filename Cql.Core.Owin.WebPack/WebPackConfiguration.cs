namespace Cql.Core.Owin.WebPack
{
    using System.Collections.Generic;
    using System.Threading;

    public static class WebPackConfiguration
    {
        private static List<int> _devServerPorts;

        private static HandleDefaultReponseDelegate _responseHandler;

        public static string DefaultFilePath { get; set; } = "/index.html";

        public static List<int> DevServerPorts
        {
            get
            {
                return LazyInitializer.EnsureInitialized(ref _devServerPorts, () => new List<int> { 8080 });
            }

            set => _devServerPorts = value;
        }

        public static HandleDefaultReponseDelegate SendDefaultResponseHandler
        {
            get => _responseHandler ?? RedirectClientUrlsToIndexMiddleware.SendDefaultResponse;
            set => _responseHandler = value;
        }
    }
}
