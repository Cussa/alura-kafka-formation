using System;
using System.Threading;
using Confluent.Kafka;
using Ecommerce.Common;

namespace Ecommerce.Service.FraudDetector
{
    class FraudDetectorConsumerFunction : IConsumerFunction<Order>
    {
        public void Consume(ConsumeResult<string, Order> record)
        {
            Console.WriteLine("------------------------------");
            Console.WriteLine("Processing new order, checking for fraud");
            Console.WriteLine($"{record.Message.Key}\n{record.Message.Value}\n{record.Partition}\n{record.Offset}");

            var order = record.Message.Value;
            Console.WriteLine($"UserId: {order.UserId}\nOrderId: {order.OrderId}\nAmount: {order.Amount}");

            //Simulate a slow process of fraud detection
            Thread.Sleep(5000);
            Console.WriteLine("Order processed");
        }
    }
}
