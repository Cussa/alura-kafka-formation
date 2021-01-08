using Confluent.Kafka;
using Ecommerce.Common.Models;

namespace Ecommerce.Common.Kafka
{
    public interface IConsumerFunction<T>
    {
        void Consume(ConsumeResult<string, KafkaMessage<T>> record);
        string Topic { get; }
        string ConsumerGroup { get; }
    }
}
