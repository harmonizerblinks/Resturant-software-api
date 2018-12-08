using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resturant.Repository;
using Resturant.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        // GET Sequence
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var sequence = _sequenceRepository.Query();

            return Ok(sequence);
        }

        // GET Sequence
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

        // POST Sequence
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Sequence value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _sequenceRepository.InsertAsync(value);

            return Created($"sequence/{value.SequenceId}", value);
        }

        // PUT Sequence
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Sequence value, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != value.SequenceId) return BadRequest();

            await _sequenceRepository.UpdateAsync(value);

            return Ok(value);
        }

        // DELETE Sequence
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var sequence = _sequenceRepository.DeleteAsync(id);

            return Ok(sequence.Result);
        }

        [HttpGet("Code/{type}")]
        public async Task<IActionResult> GetCode([FromRoute] string type)
        {
            //var sequence = await _context.Sequence.FirstOrDefaultAsync(a => a.Name.ToLower() == type.ToLower());
            var sequence = _sequenceRepository.Query().Where(a => a.Name.ToLower() == type.ToLower()).FirstOrDefault();

            if (sequence != null)
            {
                string transcodenum = (sequence.Counter).ToString();
                int padnum = sequence.Length - transcodenum.Length;
                string number = transcodenum.PadLeft(padnum + 1, '0');
                string code = sequence.Prefix + number;

                sequence.Counter += 1;
                await _sequenceRepository.UpdateAsync(sequence);
                //_context.Entry(sequence).State = EntityState.Modified;
                //await _context.SaveChangesAsync();

                return Ok(code);
            }
            return null;
        }

    }
}