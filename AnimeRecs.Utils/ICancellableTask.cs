using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnimeRecs.Utils
{
    public interface ICancellableTask
    {
        Task Task { get; }
        CancellationTokenSource CancellationTokenSource { get; }
    }

    public static class CancellableTaskExtensions
    {
        public static void Cancel(this ICancellableTask cancellableTask)
        {
            cancellableTask.CancellationTokenSource.Cancel();
        }
    }
}
