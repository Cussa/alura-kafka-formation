using System;
using Ecommerce.Common;

namespace Ecommerce.Service.NewOrder
{
    public class GenerateOrders
    {
        public static void Main(string[] args)
        {
            using var orderDispatcher = new KafkaDispatcher<Order>();
            using var emailDispatcher = new KafkaDispatcher<string>();
            var random = new Random();
            var email = random.NextDouble() + "@mailinator.com";

            for (int i = 0; i < 10; i++)
            {
                var orderId = Guid.NewGuid().ToString();
                var amount = random.NextDouble() * 5000 + 1;

                var order = new Order(orderId, amount, email);
                orderDispatcher.Send(Topics.NewOrder, email, order);

                var emailCode = "Thank you for your order! We are processing your order!";
                emailDispatcher.Send(Topics.SendEmail, email, emailCode);
            }
        }
    }
}
