using System;
using Confluent.Kafka;
using Ecommerce.Common;
using Ecommerce.Common.Config;
using Ecommerce.Common.Kafka;
using Ecommerce.Common.Models;

namespace Ecommerce.Service.EmailNewOrder
{
    class EmailNewOrderConsumerFunction : IConsumerFunction<Order>
    {
        private readonly KafkaDispatcher<string> _emailDispatcher;

        public EmailNewOrderConsumerFunction()
        {
            _emailDispatcher = new KafkaDispatcher<string>();
        }

        public void Consume(ConsumeResult<string, KafkaMessage<Order>> record)
        {
            Console.WriteLine("------------------------------");
            Console.WriteLine("Processing new order, preparing email");
            Console.WriteLine(record.ToRecordString());

            var order = record.Message.Value.Payload;
            var emailCode = "Thank you for your order! We are processing your order!";
            var id = record.Message.Value.CorrelationId.ContinueWith(nameof(EmailNewOrderConsumerFunction));
            _emailDispatcher.Send(Topics.SendEmail, order.Email, emailCode, id);
        }
    }
}
