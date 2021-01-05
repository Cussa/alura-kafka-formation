using Ecommerce.Common;

namespace Ecommerce.Service.ReadingReport
{
    class ReadingReportService
    {
        static void Main(string[] args)
        {
            var readingReportConsumerFunction = new ReadingReportConsumerFunction();
            using var service = new KafkaService<User>(
                typeof(ReadingReportService).Name,
                Topics.UserGenerateReadingReport,
                readingReportConsumerFunction,
                new JsonKafkaDeserializer<User>());
            service.Run();
        }
    }
}
