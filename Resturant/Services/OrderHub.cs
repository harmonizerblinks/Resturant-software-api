using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Resturant.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Services
{
    [Produces("application/json")]
    [AllowAnonymous]
    public class OrderHub : Hub
    {
        private readonly IOrderRepository _orderRepository;

        public OrderHub(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public void OrdersUpdate()
        {
            var order = _orderRepository.Query().Where(o => o.Status.ToLower() == "pending");

            Clients.All.SendAsync("orders", order);
        }

        public void Send(string message)
        {
            Clients.All.SendAsync("send", message);
            // RecurringJob.AddOrUpdate("Updating Screen", () => Orders(), Cron.MinuteInterval(3));
        }

        public async Task SendAsync(string message)
        {
            await Clients.All.SendAsync("send", message);
            // RecurringJob.AddOrUpdate("Updating Order", () => Orders(), Cron.Hourly);
        }

        public async Task Orders()
        {
            var pending = _orderRepository.GetAll().Where(a => a.Status.ToLower() == "pending" || a.Status.ToLower() == "in-process");
            var ready = _orderRepository.GetAll().Where(a => a.Status.ToLower() == "ready");

            await Clients.All.SendAsync("Send", "Hello From Hangfire");
            await Clients.All.SendAsync("pending", pending);
            await Clients.All.SendAsync("ready", ready);

        }

    }
}
