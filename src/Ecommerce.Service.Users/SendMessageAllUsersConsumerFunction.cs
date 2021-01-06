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

        public void Consume(ConsumeResult<string, KafkaMessage<string>> record)
        {
            var topic = record.Message.Value.Payload.Trim('"');
            Console.WriteLine("------------------------------");
            Console.WriteLine("Processing a new batch for every user");
            Console.WriteLine(record.ToRecordString());

            foreach (var user in GetAllUsers())
                _dispatcher.Send(topic, user.Uuid, user,
                    record.Message.Value.CorrelationId.ContinueWith(typeof(SendMessageAllUsersConsumerFunction).Name));
        }

        private List<User> GetAllUsers()
        {
            using var db = new UsersDbContext();
            return db.Users.ToList();
        }
    }
}
