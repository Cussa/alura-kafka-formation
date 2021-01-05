using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Confluent.Kafka;

namespace Ecommerce.Common
{
    public class KafkaService<T> : IDisposable
    {
        private readonly IConsumerFunction<T> _consumerFunction;
        private readonly IConsumer<string, T> _consumer;
        private List<TopicPartition> currentTopicPartitions = new List<TopicPartition>();

        public KafkaService(string groupId,
            string topic,
            IConsumerFunction<T> consumerFunction,
            IDeserializer<T> deserializer = null,
            Dictionary<string, string> properties = null)
        {
            var builder = new ConsumerBuilder<string, T>(GetProperties(groupId, properties));
            if (deserializer != null)
                builder.SetValueDeserializer(deserializer);

            _consumer = builder.Build();
            _consumerFunction = consumerFunction;
            _consumer.Subscribe(topic);
        }

        private void CheckAndUpdateAssignment()
        {
            if (!Enumerable.SequenceEqual(_consumer.Assignment, currentTopicPartitions))
            {
                Console.WriteLine("-----------------------------------------");
                var msg = new StringBuilder();
                foreach (var item in _consumer.Assignment)
                    msg.AppendLine($"{item.Partition}\t\t\t{item.Topic}");
                Console.WriteLine($"{_consumer.Name} subscribed to:\nPARTITION\t\tTOPIC\n{msg}");
                currentTopicPartitions = _consumer.Assignment;
                Console.WriteLine("Listening...");
                Console.WriteLine("-----------------------------------------");
            }
        }

        private ConsumerConfig GetProperties(string groupId, Dictionary<string, string> properties)
        {
            return new ConsumerConfig(properties ?? new Dictionary<string, string>())
            {
                BootstrapServers = "localhost:9092",
                GroupId = groupId,
                ClientId = Guid.NewGuid().ToString(),
                // Enable this to see the logs from Kafka connection
                //Debug = "consumer,cgrp,topic,fetch",
            };
        }

        public void Run()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };

            while (!cts.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(cts.Token);
#if DEBUG
                    CheckAndUpdateAssignment();
#endif
                    _consumerFunction.Consume(consumeResult);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("----- Operation cancelled. -----");
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        public void Dispose()
        {
            _consumer.Close();
        }
    }
}