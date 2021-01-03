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

            for (int i = 0; i < 10; i++)
            {
                var userId = Guid.NewGuid().ToString();
                var orderId = Guid.NewGuid().ToString();
                var amount = random.NextDouble() * 5000 + 1;

                var order = new Order(userId, orderId, amount);
                orderDispatcher.Send(Topics.NewOrder, userId, order);

                var email = "Thank you for your order! We are processing your order!";
                emailDispatcher.Send(Topics.SendEmail, userId, email);
            }
        }
    }
}
