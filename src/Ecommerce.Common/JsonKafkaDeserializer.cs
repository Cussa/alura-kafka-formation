using System;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace Ecommerce.Common
{
    public class JsonKafkaDeserializer<T> : IDeserializer<T>
    {
        public static JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
        {
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
        };

        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
            => JsonConvert.DeserializeObject<T>(System.Text.Encoding.UTF8.GetString(data), jsonSerializerSettings);
    }
}
