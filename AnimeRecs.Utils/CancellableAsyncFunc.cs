using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnimeRecs.Utils
{
    public struct CancellableAsyncFunc
    {
        public Func<Task> Function { get; private set; }
        public CancellationTokenSource Cancel { get; private set; }

        public CancellableAsyncFunc(Func<Task> function, CancellationTokenSource cancel)
        {
            Function = function;
            Cancel = cancel;
        }

        public CancellableTask StartTaskEnsureExceptionsWrapped()
        {
            Task task = AsyncUtils.EnsureExceptionsWrapped(Function);
            return new CancellableTask(task, Cancel);
        }
    }

    public struct CancellableAsyncFunc<T>
    {
        public Func<Task<T>> Function { get; private set; }
        public CancellationTokenSource Cancel { get; private set; }

        public CancellableAsyncFunc(Func<Task<T>> function, CancellationTokenSource cancel)
        {
            Function = function;
            Cancel = cancel;
        }

        public CancellableTask<T> StartTaskEnsureExceptionsWrapped()
        {
            Task<T> task = AsyncUtils.EnsureExceptionsWrapped(Function);
            return new CancellableTask<T>(task, Cancel);
        }
    }
}
