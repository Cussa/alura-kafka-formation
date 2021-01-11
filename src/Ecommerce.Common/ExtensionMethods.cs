using System.IO;
using System.Reflection;
using System.Threading;
using Confluent.Kafka;
using Ecommerce.Common.Models;
using Newtonsoft.Json;

namespace Ecommerce.Common
{
    public static class ExtensionMethods
    {
        public static string ToRecordString<T>(this ConsumeResult<string, KafkaMessage<T>> record)
            => $"Thread:\t{Thread.CurrentThread.ManagedThreadId}\nPartition:\t{record.Partition}\tOffset: {record.Offset}\nKey:\t\t{record.Message.Key}\nCorrelationId:\t{record.Message.Value.CorrelationId.Id}\nPayload:\t{JsonConvert.SerializeObject(record.Message.Value.Payload)}";

        public static string WithCurrentDirectory(this string filePath)
        {
            var fileEntry = Assembly.GetEntryAssembly().Location;
            var folderEntry = Path.GetDirectoryName(fileEntry);
            return Path.Combine(folderEntry, filePath);
        }
    }
}
