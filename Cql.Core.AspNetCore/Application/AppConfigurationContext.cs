using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Cql.Core.AspNetCore.Application
{
    public class AppConfigurationContext : IAppConfigurationContext
    {
        public IApplicationBuilder AppBuilder { get; set; }

        public IApplicationLifetime AppLifetime { get; set; }

        public ILoggerFactory LoggerFactory { get; set; }

        public IHostingEnvironment Env { get; set; }

        public IConfigurationRoot Configuration { get; set; }
    }
}
