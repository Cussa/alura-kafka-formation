using System;
using System.Linq;
using Confluent.Kafka;
using Ecommerce.Common;

namespace Ecommerce.Service.Users
{
    class CreateUserConsumerFunction : IConsumerFunction<Order>
    {
        public void Consume(ConsumeResult<string, KafkaMessage<Order>> record)
        {
            var order = record.Message.Value.Payload;
            Console.WriteLine("------------------------------");
            Console.WriteLine("Processing new order, checking for new user");
            Console.WriteLine(record.ToRecordString());

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
            var uuid = Guid.NewGuid().ToString();
            db.Add(new User { Uuid = uuid, Email = email });
            db.SaveChanges();

            Console.WriteLine($"User {uuid} with email {email} saved.");
        }
    }
}
