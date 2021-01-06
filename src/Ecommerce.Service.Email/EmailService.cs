using Ecommerce.Common.Config;
using Ecommerce.Common.Kafka;

namespace Ecommerce.Service.Email
{
    class EmailService
    {
        static void Main(string[] args)
        {
            var emailConsumerFunction = new EmailConsumerFunction();
            using var service = new KafkaService<string>(
                typeof(EmailService).Name,
                Topics.SendEmail,
                emailConsumerFunction);
            service.Run();
        }
    }
}
