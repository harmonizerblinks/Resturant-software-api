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
    public class NominalController : ControllerBase
    {
        private readonly INominalRepository _nominalRepository;

        public NominalController(INominalRepository nominalRepository)
        {
            _nominalRepository = nominalRepository;
        }

        // GET api/Nominal
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var nominal = _nominalRepository.Query();

            return Ok(nominal);
        }

        // GET api/Nominal
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var nominal = _nominalRepository.GetAsync(id);

            if (nominal != null)
            {
                return Ok(nominal.Result);
            }
            else
                return BadRequest();
        }

        // POST api/Nominal
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Nominal value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _nominalRepository.InsertAsync(value);

            return Created($"nominal/{value.NominalId}", value);
        }

        // PUT api/Nominal/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Nominal value, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != value.NominalId) return BadRequest();

            await _nominalRepository.UpdateAsync(value);

            return Ok(value);
        }

        // DELETE api/Nominal
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var nominal = _nominalRepository.DeleteAsync(id);

            return Ok(nominal.Result);
        }

    }
}