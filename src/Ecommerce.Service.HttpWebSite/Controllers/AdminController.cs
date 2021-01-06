using System;
using Ecommerce.Common.Config;
using Ecommerce.Common.Kafka;
using Ecommerce.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Service.HttpWebSite.Controllers
{
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly KafkaDispatcher<string> _batchDispatcher;

        public AdminController(KafkaDispatcher<string> batchDispatcher)
        {
            _batchDispatcher = batchDispatcher;
        }

        [HttpGet("/admin/generate-report")]
        public ActionResult<string> Get()
        {
            _batchDispatcher.Send(Topics.SendMessageToAllUsers,
                Topics.UserGenerateReadingReport,
                Topics.UserGenerateReadingReport,
                new CorrelationId(typeof(AdminController).Name));

            Console.WriteLine("Sent generate reports to all users");
            return Ok("Sent generate reports to all users");
        }
    }
}
