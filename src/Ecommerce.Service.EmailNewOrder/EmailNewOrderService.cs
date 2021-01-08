using System;
using Confluent.Kafka;
using Ecommerce.Common;
using Ecommerce.Common.Config;
using Ecommerce.Common.Kafka;
using Ecommerce.Common.Models;

namespace Ecommerce.Service.EmailNewOrder
{
    class EmailNewOrderService : IConsumerFunction<Order>
    {
        public string Topic => Topics.NewOrder;

        public string ConsumerGroup => nameof(EmailNewOrderService);

        private readonly KafkaDispatcher<string> _emailDispatcher;

        public EmailNewOrderService()
        {
            _emailDispatcher = new KafkaDispatcher<string>();
        }

        static void Main(string[] args)
            => new ServiceRunner<Order>(() => new EmailNewOrderService()).Start();

        public void Consume(ConsumeResult<string, KafkaMessage<Order>> record)
        {
            Console.WriteLine("------------------------------");
            Console.WriteLine("Processing new order, preparing email");
            Console.WriteLine(record.ToRecordString());

            var order = record.Message.Value.Payload;
            var emailCode = "Thank you for your order! We are processing your order!";
            var id = record.Message.Value.CorrelationId.ContinueWith(ConsumerGroup);
            _emailDispatcher.Send(Topics.SendEmail, order.Email, emailCode, id);
        }
    }
}
