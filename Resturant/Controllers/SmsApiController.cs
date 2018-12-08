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
    public class SmsApiController : ControllerBase
    {
        private readonly ISmsApiRepository _smsapiRepository;

        public SmsApiController(ISmsApiRepository smsapiRepository)
        {
            _smsapiRepository = smsapiRepository;
        }

        // GET SmsApi
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var smsapi = _smsapiRepository.Query();

            return Ok(smsapi);
        }

        // GET SmsApi
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var smsapi = _smsapiRepository.GetAsync(id);

            if (smsapi != null)
            {
                return Ok(smsapi.Result);
            }
            else
                return BadRequest();
        }

        // POST SmsApi
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SmsApi value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _smsapiRepository.InsertAsync(value);

            return Created($"sms{value.SmsApiId}", value);
        }

        // PUT SmsApi
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] SmsApi value, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != value.SmsApiId) return BadRequest();

            await _smsapiRepository.UpdateAsync(value);

            return Ok(value);
        }

        // DELETE SmsApi
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var smsapi = _smsapiRepository.DeleteAsync(id);

            return Ok(smsapi.Result);
        }
    }
}