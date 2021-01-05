using System;
using System.Collections.Generic;
using System.Linq;
using Confluent.Kafka;
using Ecommerce.Common;

namespace Ecommerce.Service.Users
{
    class SendMessageAllUsersConsumerFunction : IConsumerFunction<string>
    {
        private readonly KafkaDispatcher<User> _dispatcher = new KafkaDispatcher<User>();

        public void Consume(ConsumeResult<string, string> record)
        {
            var topic = record.Message.Value.Trim('"');
            Console.WriteLine("------------------------------");
            Console.WriteLine("Processing a new batch for every user");
            Console.WriteLine(topic);

            foreach (var user in GetAllUsers())
                _dispatcher.Send(topic, user.Uuid, user);
        }

        private List<User> GetAllUsers()
        {
            using var db = new UsersDbContext();
            return db.Users.ToList();
        }
    }
}
