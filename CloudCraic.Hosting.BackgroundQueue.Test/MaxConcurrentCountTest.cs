using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CloudCraic.Hosting.BackgroundQueue.Test
{
    public class MaxConcurrentCountTest
    {
        [Fact]
        public async Task ServiceShouldRunMaxConcurrentCountTaskWhenExistInQueue()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            BackgroundQueue queue = new BackgroundQueue((ex) => { throw ex; }, 10, 10); ;
            BackgroundQueueService queueService = new BackgroundQueueService(queue);
            int highwaterMark = 0;

            for (int i = 0; i < 20; i++)
            {
                queue.Enqueue(async (ct) =>
                {
                    highwaterMark = Math.Max(queue.ConcurrentCount, highwaterMark);
                    await Task.Delay(5);
                });
            }

            var runningService = Task.Run(async () => await queueService.StartAsync(tokenSource.Token));

            while(queue.Count > 0)
            {
                await Task.Delay(20);
            }

            highwaterMark.Should().BeGreaterThan(1);
        }
    }
}
