using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resturant.Models;
using Resturant.Repository;

namespace Resturant.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TellerController : ControllerBase
    {
        private readonly ITellerRepository _tellerRepository;

        public TellerController(ITellerRepository tellerRepository)
        {
            _tellerRepository = tellerRepository;
        }

        // GET api/Teller
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var teller = _tellerRepository.Query();

            return Ok(teller);
        }

        // GET api/Teller
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var teller = _tellerRepository.GetAsync(id.ToString());

            if (teller != null)
            {
                return Ok(teller);
            }
            else
                return BadRequest();
        }

        // POST api/Teller
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Teller value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _tellerRepository.InsertAsync(value);

            return Created($"teller/{value.TellerId}", value);
        }

        // PUT api/Teller
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Teller value, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != value.TellerId) return BadRequest();

            await _tellerRepository.UpdateAsync(value);

            return Ok(value);
        }

        // DELETE api/Teller
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var teller = _tellerRepository.DeleteAsync(id.ToString());

            return Ok(teller);
        }
    }
}