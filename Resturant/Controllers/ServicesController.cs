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

namespace Resturant.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private IHubContext<OrderHub> _order;
        private SequenceCode _get;

        public ServicesController(IOrderRepository orderRepository, IHubContext<OrderHub> order, SequenceCode get)
        {
            _orderRepository = orderRepository;
            _order = order;
            _get = get;
        }

        // GET api/Order
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //var order = _orderRepository.Query();

            RecurringJob.AddOrUpdate(() => _get.RefreshOrder(), Cron.Minutely);
            RecurringJob.AddOrUpdate(() => RefreshOrder(), Cron.Daily);
            BackgroundJob.Schedule(() => Console.WriteLine("Reliable!"), TimeSpan.FromDays(7));
            //BackgroundJob.Enqueue(() => Refresh());

            return Ok( new { Status = "Ok" });
        }

        [NonAction]
        public void RefreshOrder()
        {

            _order.Clients.All.SendAsync("Send", "Hello From Hangfire");

            //await Clients.All.SendAsync("Send", message);
        }

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