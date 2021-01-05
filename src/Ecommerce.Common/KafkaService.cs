using System;
using System.Collections.Generic;
using System.Threading;
using Confluent.Kafka;

namespace Ecommerce.Common
{
    public class KafkaService<T> : IDisposable
    {
        private readonly IConsumerFunction<T> _consumerFunction;
        private readonly IConsumer<string, T> _consumer;

        public KafkaService(string groupId,
            string topic,
            IConsumerFunction<T> consumerFunction,
            IDeserializer<T> deserializer = null,
            Dictionary<string, string> properties = null)
        {
            var builder = new ConsumerBuilder<string, T>(GetProperties(groupId, properties));
            if (deserializer != null)
                builder.SetValueDeserializer(deserializer);

#if DEBUG
            builder.SetPartitionsAssignedHandler((c, partitions) =>
            {
                Console.WriteLine($"Assigned partitions: [{string.Join(", ", partitions)}]");
            })
            .SetPartitionsRevokedHandler((c, partitions) =>
            {
                Console.WriteLine($"Revoking assignment: [{string.Join(", ", partitions)}]");
            });
#endif

            _consumer = builder.Build();
            _consumerFunction = consumerFunction;
            _consumer.Subscribe(topic);
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