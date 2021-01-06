using System;
using System.ComponentModel;
using System.Text;
using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ecommerce.Common
{
    public class JsonKafkaAdapter<T> : IDeserializer<KafkaMessage<T>>, ISerializer<KafkaMessage<T>>
    {
        public KafkaMessage<T> Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            var obj = JsonConvert.DeserializeObject<JObject>(Encoding.UTF8.GetString(data));

            var correlationId = JsonConvert.DeserializeObject<CorrelationId>(obj.GetValue("CorrelationId").ToString());
            if (typeof(T) == typeof(string))
                return new KafkaMessage<T>(correlationId, (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(obj.GetValue("Payload").ToString()));
            else
                return new KafkaMessage<T>(correlationId, JsonConvert.DeserializeObject<T>(obj.GetValue("Payload").ToString()));
        }

        public byte[] Serialize(KafkaMessage<T> data, SerializationContext context)
            => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
    }
}
