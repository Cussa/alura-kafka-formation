﻿using Ecommerce.Common.Config;
using Ecommerce.Common.Kafka;

namespace Ecommerce.Service.FraudDetector
{
    class FraudDetectorService
    {
        static void Main(string[] args)
        {
            var fraudDetectorConsumerFunction = new FraudDetectorConsumerFunction();
            using var service = new KafkaService<Order>(
                typeof(FraudDetectorService).Name,
                Topics.NewOrder,
                fraudDetectorConsumerFunction);
            service.Run();
        }
    }
}
