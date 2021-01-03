using System;
using System.Collections.Generic;
using Confluent.Kafka;

namespace Ecommerce.Common
{
    public class KafkaDispatcher<T> : IDisposable
    {
        private readonly IProducer<string, T> _producer;

        public KafkaDispatcher()
        {
            _producer = new ProducerBuilder<string, T>(GetConfig())
                .SetValueSerializer(new JsonKafkaSerializer<T>())
                .Build();
        }

        private IEnumerable<KeyValuePair<string, string>> GetConfig()
        {
            return new ProducerConfig()
            {
                BootstrapServers = "localhost:9092",
                // Enable this to see the logs from Kafka connection
                //Debug = "broker,topic,msg"
            };
        }

        public void Send(string topic, string key, T value)
        {
            static void handler(DeliveryReport<string, T> r) =>
                Console.WriteLine(!r.Error.IsError
                    ? $"Delivered message to {r.TopicPartitionOffset}"
                    : $"Delivery Error: {r.Error.Reason}");

            var message = new Message<string, T> { Key = key, Value = value };
            _producer.Produce(topic, message, handler);
        }

        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(10));
            _producer.Dispose();
        }
    }
}
