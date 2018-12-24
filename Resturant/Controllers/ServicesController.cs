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
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

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
        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> Get([FromQuery] string hub_challenge)
        //{
        //    var order = _orderRepository.GetAll().Count();

        //    //Console.WriteLine(data);

        //    return Ok(hub_challenge);
        //}

        // GET Services
        [HttpGet("Start")]
        [AllowAnonymous]
        public async Task<IActionResult> StartService()
        {
            var order = _orderRepository.GetAll().Count();
            //var _get = new MyServices();
            RecurringJob.AddOrUpdate("Updating Order Screen", () => Refresh(), Cron.Minutely);
            //RecurringJob.AddOrUpdate("Boardcast Message", () => _order.Clients.All.SendAsync("Send", "Hello From Hangfire"), Cron.Minutely);

            return Ok(new { Status = "Ok", orders = order });
        }

        // GET Sms
        [HttpGet("Order")]
        public async Task<IActionResult> Refresh()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var pending = _orderRepository.GetAll().Where(a => a.Status.ToLower() == "pending" || a.Status.ToLower() == "in-process").ToList();
            var ready = _orderRepository.GetAll().Where(a => a.Status.ToLower() == "ready").ToList();

            //List<Order> pending_response = JsonConvert.DeserializeObject<List<Order>>(pending);
            //List<Order> ready_response = JsonConvert.DeserializeObject<List<Order>>(ready);
            //var pend = JsonConvert.SerializeObject(pending);
            //var red = JsonConvert.SerializeObject(ready);

            await _order.Clients.All.SendAsync("pending", pending);
            await _order.Clients.All.SendAsync("ready", ready);
            await _order.Clients.All.SendAsync("Send", "Updating Order Screen");

            return Ok(new { pending, ready });
        }

        // GET Sms
        [HttpGet("Dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var dash = _orderRepository.GetDashboard();
            

            return Ok(dash);
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