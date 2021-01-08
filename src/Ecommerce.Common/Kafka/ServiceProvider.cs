using System;
using System.Threading;

namespace Ecommerce.Common.Kafka
{
    public class ServiceProvider<T>
    {
        private readonly Func<IConsumerFunction<T>> _consumerFunction;

        public ServiceProvider(Func<IConsumerFunction<T>> consumerFunction)
        {
            _consumerFunction = consumerFunction;
        }

        public void Call(CancellationTokenSource cancellationTokenSource)
        {
            using var service = new KafkaService<T>(_consumerFunction.Invoke());
            service.Run(cancellationTokenSource);
        }
    }
}
