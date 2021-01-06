using System;
using System.Collections.Generic;
using Confluent.Kafka;
using Ecommerce.Common.Config;
using Ecommerce.Common.Models;

namespace Ecommerce.Common.Kafka
{
    public class KafkaDispatcher<T> : IDisposable
    {
        private readonly IProducer<string, KafkaMessage<T>> _producer;

        public KafkaDispatcher()
        {
            _producer = new ProducerBuilder<string, KafkaMessage<T>>(GetConfig())
                .SetValueSerializer(new JsonKafkaAdapter<T>())
                .Build();
        }

        private IEnumerable<KeyValuePair<string, string>> GetConfig()
        {
            return new ProducerConfig()
            {
                BootstrapServers = KafkaServerConfig.BootstrapServer,
                Acks = Acks.All
                // Enable this to see the logs from Kafka connection
                //Debug = "broker,topic,msg"
            };
        }

        public void Send(string topic, string key, T value, CorrelationId correlationId)
        {
            static void handler(DeliveryReport<string, KafkaMessage<T>> r) =>
                Console.WriteLine(!r.Error.IsError
                    ? $"Delivered message to {r.TopicPartitionOffset}"
                    : $"Delivery Error: {r.Error.Reason}");

            var kafkaMessage = new KafkaMessage<T>(correlationId.ContinueWith($"_{topic}"), value);
            var message = new Message<string, KafkaMessage<T>> { Key = key, Value = kafkaMessage };
            _producer.Produce(topic, message, handler);
        }

        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(10));
            _producer.Dispose();
        }
    }
}
