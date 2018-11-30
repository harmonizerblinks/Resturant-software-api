using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resturant.Repository;
using Resturant.Services;
using Hangfire;
using Microsoft.AspNetCore.SignalR;
using Resturant.Models;
using System.Collections;

namespace Resturant.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private IOrderRepository _orderRepository;
        //private IHubContext<OrderHub> _order { get; set; }
        private IMyServices _get;

        public ServicesController(IOrderRepository orderRepository, /*IHubContext<OrderHub> order,*/ IMyServices get)
        {
            _orderRepository = orderRepository;
            //_order = order;
            _get = get;
        }

        // GET api/Services
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var order = _orderRepository.Query();
            //RecurringJob.AddOrUpdate("Updating Order Screen", () => _get.RefreshOrder(), Cron.Minutely);
            //RecurringJob.AddOrUpdate("Boardcast Message", () => _order.Clients.All.SendAsync("Send", "Hello From Hangfire"), Cron.Minutely);

            return Ok( new { Status = "Ok" });
        }
        
        //private void RefreshOrderAsync(string message)
        //{
        //    _order.Clients.All.SendAsync("Send", "Hello From Hangfire");

        //    _order.Clients.All.SendAsync("Send", message);
        //}

        //[NonAction]
        //private void Refresh()
        //{

        //    RecurringJob.AddOrUpdate(() => Console.WriteLine("Sending Message!"), Cron.Minutely);
        //    RecurringJob.AddOrUpdate(() => Console.WriteLine("Sending Message Hourly!"), Cron.Hourly);
        //    RecurringJob.AddOrUpdate(() => Console.WriteLine("Sending Message Daily!"), Cron.Daily);
        //    RecurringJob.AddOrUpdate(() => Console.WriteLine("Sending Message Weekly!"), Cron.Weekly);
        //    RecurringJob.AddOrUpdate(() => Console.WriteLine("Sending Message Monthly!"), Cron.Monthly);
        //    RecurringJob.AddOrUpdate(() => _get.RefreshOrder(), Cron.Minutely);
        //    RecurringJob.AddOrUpdate(() => _get.RefreshOrder(), Cron.Daily);
        //}

    }
}