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
using Microsoft.EntityFrameworkCore;

namespace Resturant.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private IOrderRepository _orderRepository;
        private IHubContext<OrderHub> _order { get; set; }

        public ServicesController(IOrderRepository orderRepository, IHubContext<OrderHub> order)
        {
            _orderRepository = orderRepository;
            _order = order;
        }

        // GET Services
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var order = _orderRepository.GetAll().Count();
            //var _get = new MyServices();
            RecurringJob.AddOrUpdate("Updating Order Screen", () => Refresh(), Cron.Hourly);
            //RecurringJob.AddOrUpdate("Boardcast Message", () => _order.Clients.All.SendAsync("Send", "Hello From Hangfire"), Cron.Minutely);

            return Ok( new { Status = "Ok", orders = order });
        }

        // GET Sms
        [HttpGet("Order")]
        public async Task<IActionResult> Refresh()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            List<Order> pending = _orderRepository.GetAll().Where(a => a.Status.ToLower() == "pending" || a.Status.ToLower() == "in-process").ToList();
            List<Order> ready = _orderRepository.GetAll().Where(a => a.Status.ToLower() == "ready").ToList();

            await _order.Clients.All.SendAsync("Send", "Hello From Hangfire");
            await _order.Clients.All.SendAsync("pending", pending);
            await _order.Clients.All.SendAsync("ready", ready);
            
            return Ok(new { pending, ready });
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