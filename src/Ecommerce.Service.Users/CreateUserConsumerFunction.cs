﻿using System;
using System.Linq;
using Confluent.Kafka;
using Ecommerce.Common;

namespace Ecommerce.Service.Users
{
    class CreateUserConsumerFunction : IConsumerFunction<Order>
    {
        public void Consume(ConsumeResult<string, Order> record)
        {
            var order = record.Message.Value;
            Console.WriteLine("------------------------------");
            Console.WriteLine("Processing new order, checking for new user");
            Console.WriteLine(order.ToJsonString());

            if (IsNewUser(order.Email))
            {
                InsertNewUser(order.Email);
            }
        }

        private bool IsNewUser(string email)
        {
            using var db = new UsersDbContext();
            return !db.Users.Any(x => x.Email == email);
        }

        private void InsertNewUser(string email)
        {
            using var db = new UsersDbContext();
            db.Add(new User { Email = email });
            db.SaveChanges();
        }
    }
}
