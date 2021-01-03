using System.Text;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace Ecommerce.Common
{
    public class JsonKafkaSerializer<T> : ISerializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context)
            => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
    }
}
