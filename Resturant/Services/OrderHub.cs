using Microsoft.AspNetCore.SignalR;
using Resturant.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Services
{
    public class OrderHub : Hub
    {
        private readonly IOrderRepository _orderRepository;

        public OrderHub(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task SendAsync(string message)
        {
            await Clients.All.SendAsync("Send", message);
        }
    }
}
