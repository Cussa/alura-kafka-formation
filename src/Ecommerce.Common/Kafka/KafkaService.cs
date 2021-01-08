using System;
using System.Threading;
using Confluent.Kafka;
using Ecommerce.Common.Config;
using Ecommerce.Common.Models;

namespace Ecommerce.Common.Kafka
{
    public class KafkaService<T> : IDisposable
    {
        private readonly IConsumerFunction<T> _consumerFunction;
        private readonly IConsumer<string, KafkaMessage<T>> _consumer;

        public KafkaService(IConsumerFunction<T> consumerFunction)
        {
            var builder = new ConsumerBuilder<string, KafkaMessage<T>>(GetProperties(consumerFunction.ConsumerGroup));
            builder.SetValueDeserializer(new JsonKafkaAdapter<T>());

#if DEBUG
            builder.SetPartitionsAssignedHandler((c, partitions) =>
            {
                Console.WriteLine($"----- Thread: {Thread.CurrentThread.ManagedThreadId} - Assigned partitions: [{string.Join(", ", partitions)}]");
            })
            .SetPartitionsRevokedHandler((c, partitions) =>
            {
                Console.WriteLine($"----- Thread: {Thread.CurrentThread.ManagedThreadId} - Revoking assignment: [{string.Join(", ", partitions)}]");
            });
#endif

            _consumer = builder.Build();
            _consumerFunction = consumerFunction;
            _consumer.Subscribe(consumerFunction.Topic);
        }

        private ConsumerConfig GetProperties(string groupId)
        {
            return new ConsumerConfig()
            {
                BootstrapServers = KafkaServerConfig.BootstrapServer,
                GroupId = groupId,
                ClientId = Guid.NewGuid().ToString(),
                // Enable this to see the logs from Kafka connection
                //Debug = "consumer,cgrp,topic,fetch",
            };
        }

        public void Run(CancellationTokenSource cts)
        {
            Console.WriteLine($"----- Starting consumer for topic \"{_consumerFunction.ConsumerGroup}\" - thread: {Thread.CurrentThread.ManagedThreadId} -----");

            using var deadLetter = new KafkaDispatcher<string>();

            while (!cts.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(cts.Token);

                    try
                    {
                        _consumerFunction.Consume(consumeResult);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);

                        deadLetter.Send(Topics.DeadLetter,
                            consumeResult.Message.Value.CorrelationId.Id,
                            Newtonsoft.Json.JsonConvert.SerializeObject(consumeResult.Message.Value),
                            consumeResult.Message.Value.CorrelationId.ContinueWith("DeadLetter"));

                        Environment.Exit(-1);
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine($"----- Stopping consumer for topic \"{_consumerFunction.ConsumerGroup}\" - thread: {Thread.CurrentThread.ManagedThreadId} -----");
                    Environment.Exit(0);
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