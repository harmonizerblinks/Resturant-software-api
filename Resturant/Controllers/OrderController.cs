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

namespace Resturant.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public IHubContext<OrderHub> _order;
        private IOrderRepository _orderRepository;
        private IOrderListRepository _orderlistRepository;

        public OrderController(IOrderRepository orderRepository, IOrderListRepository orderlistRepository,
            IHubContext<OrderHub> order)
        {
            _order = order;
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

        // GET api/Order
        [HttpGet("code/{code}")]
        public async Task<IActionResult> GetOrderByCode([FromRoute] string code)
        {
            var order = _orderRepository.GetAll().Where(c => c.OrderNo == code).FirstOrDefault();

            return Ok(order);
        }

        // GET api/Order
        [HttpGet("cancel/{code}")]
        public async Task<IActionResult> CancelOrder([FromRoute] string code)
        {
            var order = _orderRepository.GetAll().Where(c => c.OrderNo == code).FirstOrDefault();

            return Ok(order);
        }

        // GET api/Order
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetOrderListByOrderid([FromRoute] int id)
        {
            var order = _orderlistRepository.GetAll().Where(c => c.OrderId == id);

            return Ok(order);
        }


        // POST api/Order
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

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