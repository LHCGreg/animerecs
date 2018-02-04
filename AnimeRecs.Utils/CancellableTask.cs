using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnimeRecs.Utils
{
    public struct CancellableTask : ICancellableTask
    {
        public Task Task { get; private set; }
        public CancellationTokenSource CancellationTokenSource { get; private set; }

        public CancellableTask(Task task, CancellationTokenSource cancellationTokenSource)
        {
            Task = task;
            CancellationTokenSource = cancellationTokenSource;
        }

        public void Cancel()
        {
            CancellationTokenSource.Cancel();
        }
    }

    public struct CancellableTask<T> : ICancellableTask
    {
        public Task<T> Task { get; private set; }
        public CancellationTokenSource CancellationTokenSource { get; private set; }

        Task ICancellableTask.Task { get { return Task; } }

        public CancellableTask(Task<T> task, CancellationTokenSource cancellationTokenSource)
        {
            Task = task;
            CancellationTokenSource = cancellationTokenSource;
        }

        public void Cancel()
        {
            CancellationTokenSource.Cancel();
        }
    }
}
