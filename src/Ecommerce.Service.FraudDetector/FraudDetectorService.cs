using System;
using System.Threading;
using Confluent.Kafka;
using Ecommerce.Common;
using Ecommerce.Common.Config;
using Ecommerce.Common.Kafka;
using Ecommerce.Common.Models;

namespace Ecommerce.Service.FraudDetector
{
    class FraudDetectorService : IConsumerFunction<Order>
    {
        public string Topic => Topics.NewOrder;

        public string ConsumerGroup => nameof(FraudDetectorService);

        private readonly KafkaDispatcher<Order> _orderDispatcher;

        public FraudDetectorService()
        {
            _orderDispatcher = new KafkaDispatcher<Order>();
        }

        static void Main(string[] args)
            => new ServiceRunner<Order>(() => new FraudDetectorService()).Start();

        public void Consume(ConsumeResult<string, KafkaMessage<Order>> record)
        {
            Console.WriteLine("------------------------------");
            Console.WriteLine("Processing new order, checking for fraud");
            Console.WriteLine(record.ToRecordString());

            var order = record.Message.Value.Payload;

            //Simulate a slow process of fraud detection
            Thread.Sleep(5000);

            if (IsFraud(order))
            {
                Console.WriteLine("Order is a fraud!!!!");
                _orderDispatcher.Send(Topics.OrderReject, order.Email, order,
                    record.Message.Value.CorrelationId.ContinueWith(ConsumerGroup));
            }
            else
            {
                Console.WriteLine("Approved: " + order);
                _orderDispatcher.Send(Topics.OrderApproved, order.Email, order,
                    record.Message.Value.CorrelationId.ContinueWith(ConsumerGroup));
            }
        }

        public bool IsFraud(Order order)
        {
            // pretend that the fraud happens when the amount is greather or equal than 4500
            return order.Amount >= 4500;
        }
    }
}
