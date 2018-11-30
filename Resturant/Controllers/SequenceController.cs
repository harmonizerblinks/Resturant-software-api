﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resturant.Repository;
using Resturant.Models;

namespace Resturant.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class SequenceController : ControllerBase
    {
        private ISequenceRepository _sequenceRepository { get; set; }

        public SequenceController(ISequenceRepository sequenceRepository)
        {
            _sequenceRepository = sequenceRepository;
        }

        // GET api/Sequence
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var sequence = _sequenceRepository.Query();

            return Ok(sequence);
        }

        // GET api/Sequence
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var sequence = _sequenceRepository.GetAsync(id);

            if (sequence != null)
            {
                return Ok(sequence.Result);
            }
            else
                return BadRequest();
        }

        // POST api/Sequence
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Sequence value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _sequenceRepository.InsertAsync(value);

            return Created($"sequence/{value.SequenceId}", value);
        }

        // PUT api/Sequence
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Sequence value, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != value.SequenceId) return BadRequest();

            await _sequenceRepository.UpdateAsync(value);

            return Ok(value);
        }

        // DELETE api/Sequence
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var sequence = _sequenceRepository.DeleteAsync(id);

            return Ok(sequence.Result);
        }

    }
}