﻿using System;
using Ecommerce.Common.Config;
using Ecommerce.Common.Kafka;
using Ecommerce.Common.Models;

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

                var id = new CorrelationId(nameof(GenerateOrders));

                var order = new Order(orderId, amount, email);
                orderDispatcher.Send(Topics.NewOrder, email, order, id);
            }
        }
    }
}
