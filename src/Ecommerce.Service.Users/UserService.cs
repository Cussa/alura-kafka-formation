using System;
using Ecommerce.Common;

namespace Ecommerce.Service.Users
{
    class UserService
    {
        static void Main(string[] args)
        {
            var mode = string.Empty;
            if (args.Length > 0)
                mode = args[0];

            ChooseService(mode);
        }

        private static void ChooseService(string service)
        {
            switch (service)
            {
                case "1":
                    Console.WriteLine("Starting User Service on the Create New User Mode");
                    CreateUserService();
                    break;
                case "2":
                    Console.WriteLine("Starting User Service on the Batch Mode");
                    BatchService();
                    break;
                default:
                    ChooseService();
                    break;
            }
        }

        private static void ChooseService()
        {
            Console.WriteLine("1\tCreate User Service");
            Console.WriteLine("2\tBatch Service");
            var choose = Console.ReadLine();
            ChooseService(choose);
        }

        private static void CreateUserService()
        {
            var createUserConsumerFunction = new CreateUserConsumerFunction();
            using var service = new KafkaService<Order>(
                typeof(UserService).Name,
                Topics.NewOrder,
                createUserConsumerFunction);
            service.Run();
        }

        private static void BatchService()
        {
            var sendMessageAllUsersConsumerFunction = new SendMessageAllUsersConsumerFunction();
            using var service = new KafkaService<string>(
                typeof(UserService).Name,
                Topics.SendMessageToAllUsers,
                sendMessageAllUsersConsumerFunction);
            service.Run();
        }
    }
}
