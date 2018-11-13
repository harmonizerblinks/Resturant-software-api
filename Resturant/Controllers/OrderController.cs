using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resturant.Repository;
using Resturant.Models;

namespace Resturant.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // GET api/Order
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var order = _orderRepository.Query();

            return Ok(order);
        }

        // GET api/Order
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var order = _orderRepository.GetAsync(id.ToString());

            if (order != null)
            {
                return Ok(order);
            }
            else
                return BadRequest();
        }

        // POST api/Order
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _orderRepository.InsertAsync(value);

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
            var order = _orderRepository.DeleteAsync(id.ToString());

            return Ok(order);
        }
    }
}