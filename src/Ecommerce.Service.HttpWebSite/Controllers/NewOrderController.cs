﻿using System;
using Ecommerce.Common.Config;
using Ecommerce.Common.Kafka;
using Ecommerce.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Service.HttpWebSite.Controllers
{
    [ApiController]
    public class NewOrderController : ControllerBase
    {
        private readonly KafkaDispatcher<Order> _orderDispatcher;
        private readonly KafkaDispatcher<string> _emailDispatcher;

        public NewOrderController(KafkaDispatcher<Order> orderDispatcher, KafkaDispatcher<string> emailDispatcher)
        {
            _orderDispatcher = orderDispatcher;
            _emailDispatcher = emailDispatcher;
        }

        [HttpGet("/new")]
        public ActionResult<string> Get([FromQuery] string email, [FromQuery] double amount)
        {
            var id = new CorrelationId(nameof(NewOrderController));

            var orderId = Guid.NewGuid().ToString();
            var order = new Order(orderId, amount, email);
            _orderDispatcher.Send(Topics.NewOrder, email, order, id);

            Console.WriteLine("New order sent succesfully");
            return Ok("New order sent succesfully");
        }
    }
}
