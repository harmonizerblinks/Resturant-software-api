using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resturant.Models;
using Resturant.Repository;

namespace Resturant.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountController(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        // GET api/Discount
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var discount = _discountRepository.Query();

            return Ok(discount);
        }

        // GET api/Discount
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var discount = _discountRepository.GetAsync(id.ToString());

            if (discount != null)
            {
                return Ok(discount);
            }
            else
                return BadRequest();
        }

        // POST api/Discount
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Discount value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _discountRepository.InsertAsync(value);

            return Created($"discount/{value.DiscountId}", value);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Discount value, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != value.DiscountId) return BadRequest();

            await _discountRepository.UpdateAsync(value);

            return Ok(value);
        }

        // DELETE api/Discount
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var discount = _discountRepository.DeleteAsync(id.ToString());

            return Ok(discount);
        }

    }
}