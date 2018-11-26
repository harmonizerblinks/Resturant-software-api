using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Resturant.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Services
{
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
        }

        public async Task SendAsync(string message)
        {
            await Clients.All.SendAsync("send", message);
        }

        public async Task Orders()
        {
            var order = _orderRepository.Query().Where(o => o.Status.ToLower() == "pending");

            await Clients.All.SendAsync("orders", order);

        }

    }
}
