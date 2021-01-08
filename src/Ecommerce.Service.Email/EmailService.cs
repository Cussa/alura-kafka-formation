using System;
using System.Threading;
using Confluent.Kafka;
using Ecommerce.Common;
using Ecommerce.Common.Config;
using Ecommerce.Common.Kafka;
using Ecommerce.Common.Models;

namespace Ecommerce.Service.Email
{
    class EmailService : IConsumerFunction<string>
    {
        public string Topic => Topics.SendEmail;

        public string ConsumerGroup => nameof(EmailService);

        static void Main(string[] args)
            => new ServiceRunner<string>(() => new EmailService()).Start();

        public void Consume(ConsumeResult<string, KafkaMessage<string>> record)
        {
            Console.WriteLine("------------------------------");
            Console.WriteLine("Sending email");
            Console.WriteLine(record.ToRecordString());

            //Simulate the email to be sent
            Thread.Sleep(1000);
            Console.WriteLine("Email sent");
        }
    }
}
