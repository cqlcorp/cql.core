using Microsoft.Extensions.DependencyInjection;

namespace Cql.Core.AspNetCore.BackgroundTasks
{
  public  static class AppBuilderExtensions
    {
        public static void AddBackgroundTasks(this IServiceCollection services)
        {
            services.AddHostedService<QueuedHostedService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
        }
    }
}
