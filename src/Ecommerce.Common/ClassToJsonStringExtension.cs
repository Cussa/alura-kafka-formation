using Confluent.Kafka;
using Newtonsoft.Json;

namespace Ecommerce.Common
{
    public static class ClassToJsonStringExtension
    {
        public static string ToJsonString(this object data) => JsonConvert.SerializeObject(data);

        public static string ToRecordString<T>(this ConsumeResult<string, KafkaMessage<T>> record)
            => $"Partition:\t{record.Partition}\tOffset: {record.Offset}\nKey:\t\t{record.Message.Key}\nCorrelationId:\t{record.Message.Value.CorrelationId.Id}\nPayload:\t{record.Message.Value.Payload.ToJsonString()}";
    }
}
