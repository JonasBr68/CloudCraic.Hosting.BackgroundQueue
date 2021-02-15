using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CloudCraic.Hosting.BackgroundQueue
{
    public interface IBackgroundQueue
    {
        int Count { get; }
        int ConcurrentCount { get; }
        int MaxConcurrentCount { get; }

        int MillisecondsToWaitBeforePickingUpTask { get; }

        Task Dequeue(CancellationToken cancellationToken);

        void Enqueue(Func<CancellationToken, Task> task);
    }
}
