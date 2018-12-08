using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resturant.Repository;
using Resturant.Models;

namespace Resturant.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepository _locationRepository;

        public LocationController(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        // GET Location
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var location = _locationRepository.GetAll();

            return Ok(location);
        }

        // GET Location
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var location = _locationRepository.GetAsync(id);

            if (location != null)
            {
                return Ok(location.Result);
            }
            else
                return BadRequest();
        }

        // POST Location
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Location value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _locationRepository.InsertAsync(value);

            return Created($"location/{value.LocationId}", value);
        }

        // PUT Location
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Location value, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != value.LocationId) return BadRequest();

            await _locationRepository.UpdateAsync(value);

            return Ok(value);
        }

        // DELETE Location
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var location = _locationRepository.DeleteAsync(id);

            return Ok(location.Result);
        }
    }
}