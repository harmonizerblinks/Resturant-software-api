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
    public class SmsController : ControllerBase
    {
        private readonly ISmsRepository _smsRepository;

        public SmsController(ISmsRepository smsRepository)
        {
            _smsRepository = smsRepository;
        }

        // GET api/Sms
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var sms = _smsRepository.Query();

            return Ok(sms);
        }

        // GET api/Sms
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var sms = _smsRepository.GetAsync(id);

            if (sms != null)
            {
                return Ok(sms.Result);
            }
            else
                return BadRequest();
        }

        // POST api/Sms
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Sms value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _smsRepository.InsertAsync(value);

            return Created($"sms/{value.SmsId}", value);
        }

        // PUT api/Sms
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Sms value, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != value.SmsId) return BadRequest();

            await _smsRepository.UpdateAsync(value);

            return Ok(value);
        }

        // DELETE api/Sms
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var sms = _smsRepository.DeleteAsync(id);

            return Ok(sms.Result);
        }
    }
}