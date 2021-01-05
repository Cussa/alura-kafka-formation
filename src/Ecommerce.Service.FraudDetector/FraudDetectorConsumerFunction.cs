using System;
using System.Threading;
using Confluent.Kafka;
using Ecommerce.Common;

namespace Ecommerce.Service.FraudDetector
{
    class FraudDetectorConsumerFunction : IConsumerFunction<Order>
    {
        private readonly KafkaDispatcher<Order> _orderDispatcher;

        public FraudDetectorConsumerFunction()
        {
            _orderDispatcher = new KafkaDispatcher<Order>();
        }

        public void Consume(ConsumeResult<string, Order> record)
        {
            Console.WriteLine("------------------------------");
            Console.WriteLine("Processing new order, checking for fraud");
            Console.WriteLine($"{record.Message.Key}\n{record.Message.Value}\n{record.Partition}\n{record.Offset}");

            var order = record.Message.Value;
            Console.WriteLine($"Order: {order}");

            //Simulate a slow process of fraud detection
            Thread.Sleep(5000);

            if (IsFraud(order))
            {
                Console.WriteLine("Order is a fraud!!!!");
                _orderDispatcher.Send(Topics.OrderReject, order.Email, order);
            }
            else
            {
                Console.WriteLine("Approved: " + order);
                _orderDispatcher.Send(Topics.OrderApproved, order.Email, order);
            }
        }

        public bool IsFraud(Order order)
        {
            // pretend that the fraud happens when the amount is greather or equal than 4500
            return order.Amount >= 4500;
        }
    }
}
