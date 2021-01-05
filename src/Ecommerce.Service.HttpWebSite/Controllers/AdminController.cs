using System;
using Ecommerce.Common;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Service.HttpWebSite.Controllers
{
    [ApiController]
    public class AdminrController : ControllerBase
    {
        private readonly KafkaDispatcher<string> _batchDispatcher;

        public AdminrController(KafkaDispatcher<string> batchDispatcher)
        {
            _batchDispatcher = batchDispatcher;
        }

        [HttpGet("/admin/generate-report")]
        public ActionResult<string> Get()
        {
            _batchDispatcher.Send(Topics.SendMessageToAllUsers, Topics.UserGenerateReadingReport, Topics.UserGenerateReadingReport);

            Console.WriteLine("Sent generate reports to all users");
            return Ok("Sent generate reports to all users");
        }
    }
}
