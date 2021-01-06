using System;
using System.IO;
using Confluent.Kafka;
using Ecommerce.Common;
using Ecommerce.Common.Kafka;
using Ecommerce.Common.Models;

namespace Ecommerce.Service.ReadingReport
{
    class ReadingReportConsumerFunction : IConsumerFunction<User>
    {
        public void Consume(ConsumeResult<string, KafkaMessage<User>> record)
        {
            var user = record.Message.Value.Payload;
            Console.WriteLine("------------------------------");
            Console.WriteLine($"Processing report for {user.Uuid}");
            Console.WriteLine(record.ToRecordString());

            var folder = AppContext.BaseDirectory;
            var sourcePath = Path.Combine(folder, "ReportModel.txt");
            var targetFolder = Path.Combine(folder, "target");
            Directory.CreateDirectory(targetFolder);

            var targetPath = Path.Combine(targetFolder, user.Uuid + ".txt");
            File.Copy(sourcePath, targetPath, true);
            File.AppendAllLines(targetPath, new[] { $"Generated in {DateTime.Now}", $"Created for the user {user.Uuid}" });
            Console.WriteLine($"File created: {targetPath}");
        }
    }
}
