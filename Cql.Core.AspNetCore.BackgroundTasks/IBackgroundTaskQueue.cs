using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cql.Core.AspNetCore.BackgroundTasks
{
    public interface IBackgroundTaskQueue
    {
        void QueueBackgroundWorkItem(string taskName, Func<CancellationToken, Task> workItem, int? delayInMilliseconds);

        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}
