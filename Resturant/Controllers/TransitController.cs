using System;
using System.Collections.Generic;
using System.Linq;
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
    public class TransitController : ControllerBase
    {
        private readonly ITransitRepository _transitRepository;

        public TransitController(ITransitRepository transitRepository)
        {
            _transitRepository = transitRepository;
        }

        // GET Transit
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var transit = _transitRepository.GetAll();

            return Ok(transit);
        }

        // GET Transit
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var transit = _transitRepository.GetAsync(id);

            if (transit != null)
            {
                return Ok(transit.Result);
            }
            else
                return BadRequest();
        }

        // POST Transit
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Transit value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _transitRepository.InsertAsync(value);
            
            value = _transitRepository.GetAll().Where(n=>n.TransitId == value.TransitId).FirstOrDefault();

            return Created($"transit/{value.TransitId}", value);
        }

        // PUT Transit
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Transit value, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != value.TransitId) return BadRequest();

            await _transitRepository.UpdateAsync(value);

            return Ok(value);
        }

        // DELETE Transit
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var transit = _transitRepository.DeleteAsync(id);

            return Ok(transit.Result);
        }
    }
}