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
