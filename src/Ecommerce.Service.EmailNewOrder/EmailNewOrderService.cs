using Ecommerce.Common.Config;
using Ecommerce.Common.Kafka;

namespace Ecommerce.Service.EmailNewOrder
{
    class EmailNewOrderService
    {
        static void Main(string[] args)
        {
            var emailNewOrderConsumerFunction = new EmailNewOrderConsumerFunction();
            using var service = new KafkaService<Order>(
                typeof(EmailNewOrderService).Name,
                Topics.NewOrder,
                emailNewOrderConsumerFunction);
            service.Run();
        }
    }
}
