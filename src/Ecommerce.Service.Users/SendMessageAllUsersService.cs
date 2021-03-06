﻿using System;
using System.Collections.Generic;
using System.Linq;
using Confluent.Kafka;
using Ecommerce.Common;
using Ecommerce.Common.Config;
using Ecommerce.Common.Kafka;
using Ecommerce.Common.Models;

namespace Ecommerce.Service.Users
{
    class SendMessageAllUsersService : IConsumerFunction<string>
    {
        private readonly KafkaDispatcher<User> _dispatcher = new KafkaDispatcher<User>();

        public string Topic => Topics.SendMessageToAllUsers;

        public string ConsumerGroup => nameof(SendMessageAllUsersService);

        public void Consume(ConsumeResult<string, KafkaMessage<string>> record)
        {
            var topic = record.Message.Value.Payload.Trim('"');
            Console.WriteLine("------------------------------");
            Console.WriteLine("Processing a new batch for every user");
            Console.WriteLine(record.ToRecordString());

            foreach (var user in GetAllUsers())
                _dispatcher.Send(topic, user.Uuid, user,
                    record.Message.Value.CorrelationId.ContinueWith(ConsumerGroup));
        }

        private List<User> GetAllUsers()
        {
            using var db = new UsersDbContext();
            return db.Users.ToList();
        }
    }
}
