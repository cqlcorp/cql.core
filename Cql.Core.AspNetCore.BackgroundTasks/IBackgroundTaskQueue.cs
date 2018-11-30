using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cql.Core.AspNetCore.BackgroundTasks
{
    public interface IBackgroundTaskQueue
    {
        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
        
        void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem);

        void QueueBackgroundWorkItem(string taskName, Func<CancellationToken, Task> workItem);

        void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem, int? delayMilliseconds);

        void QueueBackgroundWorkItem(string taskName, Func<CancellationToken, Task> workItem, int? delayMilliseconds);
    }
}
