using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resturant.Repository;
using Resturant.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.SignalR;
using Resturant.Services;
using System;

namespace Resturant.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public IHubContext<OrderHub> _order;
        private Sequences _get;
        private IOrderRepository _orderRepository;
        private IOrderListRepository _orderlistRepository;
        private readonly ITellerRepository _tellerRepository;
        private readonly ITransitRepository _transitRepository;
        private readonly ITransactionRepository _transactionRepository;

        public OrderController(IOrderRepository orderRepository, IOrderListRepository orderlistRepository,
            IHubContext<OrderHub> order, Sequences get, ITransitRepository transitRepository,
            ITransactionRepository transactionRepository, ITellerRepository tellerRepository)
        {
            _get = get;
            _order = order;
            _tellerRepository = tellerRepository;
            _tellerRepository = tellerRepository;
            _orderRepository = orderRepository;
            _orderlistRepository = orderlistRepository;
        }

        // GET api/Order
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var order = _orderRepository.GetAll();

            return Ok(order);
        }

        // GET api/Order
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var order = _orderRepository.GetAsync(id);

            if (order != null)
            {
                return Ok(order.Result);
            }
            else
                return BadRequest();
        }

        // GET api/Order/Code/ORD344
        [HttpGet("Code/{code}")]
        public async Task<IActionResult> GetOrderByCode([FromRoute] string code)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var order = _orderRepository.GetAll().Where(c => c.OrderNo == code).FirstOrDefault();

            return Ok(order);
        }

        // GET api/Order/Summary/userid
        [HttpGet("Summary/{id}")]
        public async Task<IActionResult> GetOrderSummary([FromRoute] string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var order = _orderRepository.GetTodayOrders().Where(c => c.UserId == id);

            return Ok(order);
        }

        // GET api/Order/cancel/5
        [HttpGet("Cancel/{code}")]
        public async Task<IActionResult> CancelOrder([FromRoute] int code)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var order = _orderRepository.GetAll().Where(c => c.OrderId == code).FirstOrDefault();
            order.Status = "Cancel";
            await _orderRepository.UpdateAsync(order);
            await _get.RefreshOrder();

            return Ok(order);
        }

        // GET api/Order/confirm/5
        [HttpGet("Confirm/{code}")]
        public async Task<IActionResult> ConfirmOrder([FromRoute] int code)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var order = _orderRepository.GetAll().Where(c => c.OrderId == code).FirstOrDefault();
            order.Status = "InProcess";
            await _orderRepository.UpdateAsync(order);
            await _get.RefreshOrder();

            return Ok(order);
        }
        
        // GET api/Order/confirm/5
        [HttpGet("Ready/{code}")]
        public async Task<IActionResult> FinishOrder([FromRoute] int code)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var order = _orderRepository.GetAll().Where(c => c.OrderId == code).FirstOrDefault();
            order.Status = "Ready";
            await _orderRepository.UpdateAsync(order);
            await _get.RefreshOrder();
            return Ok(order);
        }

        // GET api/Order/confirm/5
        [HttpGet("Delivered/{code}")]
        public async Task<IActionResult> DeliverOrder([FromRoute] int code)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var order = _orderRepository.GetAll().Where(c => c.OrderId == code).FirstOrDefault();
            order.Status = "Delivered";
            await _orderRepository.UpdateAsync(order);
            await _get.RefreshOrder();
            return Ok(order);
        }

        // GET api/Order
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetOrderListByOrderid([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var order = _orderlistRepository.GetAll().Where(c => c.OrderId == id);

            return Ok(order);
        }
        
        // POST api/Order
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            value.OrderNo = await _get.GetCode("Order");
            await _orderRepository.InsertAsync(value);

            foreach(var l in value.Orderlist)
            {
                l.OrderId = value.OrderId;
            }
            await _orderlistRepository.InsertRangeAsync(value.Orderlist);

            return Created($"order/{value.OrderId}", value);
        }

        // PUT api/Order
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Order value, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != value.OrderId) return BadRequest();

            await _orderRepository.UpdateAsync(value);

            return Ok(value);
        }

        // DELETE api/Order
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var order = _orderRepository.DeleteAsync(id);

            return Ok(order.Result);
        }
    }
}