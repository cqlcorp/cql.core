using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cql.Core.AspNetCore.Application
{
    public class ServiceConfigurationContext : IServiceConfigurationContext
    {
        public IServiceCollection Services { get; set; }

        public IConfigurationRoot Configuration { get; set; }
    }
}
