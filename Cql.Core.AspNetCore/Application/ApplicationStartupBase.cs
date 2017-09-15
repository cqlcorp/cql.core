using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Cql.Core.AspNetCore.Application
{
    public abstract class ApplicationStartupBase
    {
        protected ApplicationStartupBase(IHostingEnvironment env)
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            Configuration = Step_01_BuildConfiguration(env);
        }

        public IConfigurationRoot Configuration { get; }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IApplicationLifetime appLifetime)
        {
            var context = new AppConfigurationContext
            {
                Configuration = Configuration,
                AppBuilder = app,
                AppLifetime = appLifetime,
                Env = env,
                LoggerFactory = loggerFactory
            };

            Step_03_Configure(context);
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var context = new ServiceConfigurationContext
            {
                Configuration = Configuration,
                Services = services
            };

            return Step_02_ConfigureServices(context);
        }

        protected abstract IConfigurationRoot Step_01_BuildConfiguration(IHostingEnvironment env);

        protected abstract IServiceProvider Step_02_ConfigureServices(IServiceConfigurationContext context);

        protected abstract void Step_03_Configure(IAppConfigurationContext context);
    }
}
