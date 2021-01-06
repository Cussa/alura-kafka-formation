namespace Ecommerce.Common.Models
{
    public class KafkaMessage<T>
    {
        public CorrelationId CorrelationId { get; }
        public T Payload { get; }

        public KafkaMessage(CorrelationId id, T payload)
        {
            CorrelationId = id;
            Payload = payload;
        }
    }
}
