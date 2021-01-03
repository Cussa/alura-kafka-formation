using System;
using System.Threading;
using Confluent.Kafka;
using Ecommerce.Common;

namespace Ecommerce.Service.Email
{
    public class EmailConsumerFunction : IConsumerFunction<string>
    {
        public void Consume(ConsumeResult<string, string> record)
        {
            Console.WriteLine("------------------------------");
            Console.WriteLine("Sending email");
            Console.WriteLine($"{record.Message.Key}\n{record.Message.Value}\n{record.Partition}\n{record.Offset}");

            //Simulate the email to be sent
            Thread.Sleep(1000);
            Console.WriteLine("Email sent");
        }
    }
}
