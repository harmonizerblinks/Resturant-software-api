using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resturant.Models;
using Resturant.Repository;

namespace Resturant.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISalesRepository _salesRepository;

        public SalesController(ISalesRepository salesRepository)
        {
            _salesRepository = salesRepository;
        }

        // GET api/Sales
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var sales = _salesRepository.Query();

            return Ok(sales);
        }

        // GET api/Sales
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var sales = _salesRepository.GetAsync(id);

            if (sales != null)
            {
                return Ok(sales.Result);
            }
            else
                return BadRequest();
        }

        // POST api/Sales
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Sales value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _salesRepository.InsertAsync(value);

            return Created($"sales/{value.SalesId}", value);
        }

        // PUT api/Sales
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Sales value, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != value.SalesId) return BadRequest();

            await _salesRepository.UpdateAsync(value);

            return Ok(value);
        }

        // DELETE api/Sales
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var sales = _salesRepository.DeleteAsync(id);

            return Ok(sales.Result);
        }
    }
}