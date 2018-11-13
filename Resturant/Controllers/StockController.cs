using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resturant.Repository;
using Resturant.Models;

namespace Resturant.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;

        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        // GET api/Stock
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var stock = _stockRepository.Query();

            return Ok(stock);
        }

        // GET api/Stock
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var stock = _stockRepository.GetAsync(id.ToString());

            if (stock != null)
            {
                return Ok(stock);
            }
            else
                return BadRequest();
        }

        // POST api/Stock
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Stock value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _stockRepository.InsertAsync(value);

            return Created($"stock/{value.StockId}", value);
        }

        // PUT api/Stock
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Stock value, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != value.StockId) return BadRequest();

            await _stockRepository.UpdateAsync(value);

            return Ok(value);
        }

        // DELETE api/Stock
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var stock = _stockRepository.DeleteAsync(id.ToString());

            return Ok(stock);
        }
    }
}