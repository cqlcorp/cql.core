using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cql.Core.AspNetCore.Application
{
    public interface IServiceConfigurationContext
    {
        IServiceCollection Services { get; set; }

        IConfigurationRoot Configuration { get; set; }
    }
}