using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Cql.Core.AspNetCore.Application
{
    public interface IAppConfigurationContext
    {
        IApplicationBuilder AppBuilder { get; set; }

        IApplicationLifetime AppLifetime { get; set; }

        ILoggerFactory LoggerFactory { get; set; }

        IHostingEnvironment Env { get; set; }

        IConfigurationRoot Configuration { get; set; }
    }
}
