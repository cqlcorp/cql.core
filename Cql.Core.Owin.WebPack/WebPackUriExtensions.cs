using System;
using System.Linq;

namespace Cql.Core.Owin.WebPack
{
    public static class WebPackUriExtensions
    {
        public static bool IsWebpackDevServer(this Uri uri)
        {
            return IsWebpackDevServer(uri?.Port);
        }

        public static bool IsWebpackDevServer(this int? port)
        {
            return WebPackConfiguration.DevServerPorts.Any(devServerPort =>  port.GetValueOrDefault() == devServerPort);
        }
    }
}
