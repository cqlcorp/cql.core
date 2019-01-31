using System;
using System.Collections.Concurrent;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Cql.Core.AspNetCore.BackgroundTasks
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly ILogger _logger;
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);
        private readonly ConcurrentQueue<Func<CancellationToken, Task>> _workItems = new ConcurrentQueue<Func<CancellationToken, Task>>();

        public BackgroundTaskQueue(ILoggerFactory loggerFactory, IHttpContextAccessor httpAccessor)
        {
            _logger = loggerFactory.CreateLogger<BackgroundTaskQueue>();
            _httpAccessor = httpAccessor;
        }

        public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);

            _workItems.TryDequeue(out var workItem);

            return workItem;
        }

        public void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem)
        {
            QueueBackgroundWorkItem(workItem, null);
        }

        public void QueueBackgroundWorkItem(string taskName, Func<CancellationToken, Task> workItem)
        {
            QueueBackgroundWorkItem(taskName, workItem, null);
        }
        
        public void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem, int? delayMilliseconds)
        {
            QueueBackgroundWorkItem(null, workItem, delayMilliseconds);
        }

        public void QueueBackgroundWorkItem(string taskName, Func<CancellationToken, Task> workItem, int? delayMilliseconds)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            if (string.IsNullOrEmpty(taskName))
            {
                taskName = $"A task queued at {DateTimeOffset.Now:o}";
            }

            LogQueued(taskName);

            IPrincipal currentUser = GetCurrentUser();

            async Task BackgroundTask(CancellationToken cancellationToken)
            {
                if (delayMilliseconds.HasValue)
                {
                    await Task.Delay(delayMilliseconds.Value, cancellationToken);
                }

                await RunTaskAsync(currentUser, taskName, workItem, cancellationToken);
            }

            _workItems.Enqueue(BackgroundTask);

            _signal.Release();
        }

        private IPrincipal GetCurrentUser()
        {
            return _httpAccessor.HttpContext?.User ?? Thread.CurrentPrincipal;
        }

        private void LogCompleted(string taskName)
        {
            _logger.LogInformation($"BackgroundTaskCompleted:{taskName}");
        }

        private void LogError(string taskName, Exception exception)
        {
            _logger.LogError(
"*******************************************************************\n" +
$"BackgroundTaskError:{taskName}\n" +
$"{exception}\n" +
"*******************************************************************");
        }

        private void LogQueued(string taskName)
        {
            _logger.LogInformation($"BackgroundTaskQueued:{taskName}");
        }

        private void LogStarted(string taskName)
        {
            _logger.LogInformation($"BackgroundTaskStarted:{taskName}");
        }

        private void OnCompleted(string taskName)
        {
            LogCompleted(taskName);
        }

        private void OnError(string taskName, Exception exception)
        {
            LogError(taskName, exception);
        }

        private void OnStarted(string taskName)
        {
            LogStarted(taskName);
        }

        private async Task RunTaskAsync(IPrincipal user, string taskName, Func<CancellationToken, Task> task, CancellationToken cancellationToken)
        {
            try
            {
                OnStarted(taskName);

                await Task.Run(async () =>
                    {
                        if (user != null)
                        {
                            Thread.CurrentPrincipal = user;
                        }

                        await task(cancellationToken);
                    },
                    cancellationToken);

                OnCompleted(taskName);
            }
            catch (Exception ex)
            {
                OnError(taskName, ex);
            }
        }
    }
}
