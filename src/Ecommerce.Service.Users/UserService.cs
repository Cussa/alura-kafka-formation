using System;
using Ecommerce.Common.Kafka;

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
            => new ServiceRunner<Order>(() => new CreateUserService()).Start();

        private static void BatchService()
            => new ServiceRunner<string>(() => new SendMessageAllUsersService()).Start();
    }
}
