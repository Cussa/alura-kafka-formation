using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Common.Kafka
{
    public class ServiceRunner<T>
    {
        private readonly ServiceProvider<T> _provider;

        public ServiceRunner(Func<IConsumerFunction<T>> consumerFunction)
            => _provider = new ServiceProvider<T>(consumerFunction);

        public void Start() => Start(5);
        public void Start(int threadCount)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };

            List<Task> tasksRunning = new List<Task>();
            for (int i = 0; i < threadCount; i++)
                tasksRunning.Add(Task.Run(() => StartThread(cts)));

            while (!cts.IsCancellationRequested || tasksRunning.Count > 0)
                Thread.Sleep(1000);
        }

        private void StartThread(CancellationTokenSource cts) => _provider.Call(cts);
    }
}
