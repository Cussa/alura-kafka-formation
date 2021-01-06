using Confluent.Kafka;
using Ecommerce.Common.Models;
using Newtonsoft.Json;

namespace Ecommerce.Common
{
    public static class ExtensionMethods
    {
        public static string ToRecordString<T>(this ConsumeResult<string, KafkaMessage<T>> record)
            => $"Partition:\t{record.Partition}\tOffset: {record.Offset}\nKey:\t\t{record.Message.Key}\nCorrelationId:\t{record.Message.Value.CorrelationId.Id}\nPayload:\t{JsonConvert.SerializeObject(record.Message.Value.Payload)}";
    }
}
