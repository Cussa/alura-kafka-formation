using System;
using System.Collections.Generic;
using System.Text;
using Ecommerce.Common;

namespace Ecommerce.Service.Users
{
    class CreateUserService
    {
        static void Main(string[] args)
        {
            var createUserConsumerFunction = new CreateUserConsumerFunction();
            using var service = new KafkaService<Order>(
                typeof(CreateUserService).Name,
                Topics.NewOrder,
                createUserConsumerFunction,
                new JsonKafkaDeserializer<Order>());
            service.Run();
        }
    }
}
