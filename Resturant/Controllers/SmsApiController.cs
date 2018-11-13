﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resturant.Repository;
using Resturant.Models;

namespace Resturant.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SmsApiController : ControllerBase
    {
        private readonly ISmsApiRepository _smsapiRepository;

        public SmsApiController(ISmsApiRepository smsapiRepository)
        {
            _smsapiRepository = smsapiRepository;
        }

        // GET api/SmsApi
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var smsapi = _smsapiRepository.Query();

            return Ok(smsapi);
        }

        // GET api/SmsApi
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var smsapi = _smsapiRepository.GetAsync(id.ToString());

            if (smsapi != null)
            {
                return Ok(smsapi);
            }
            else
                return BadRequest();
        }

        // POST api/SmsApi
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SmsApi value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _smsapiRepository.InsertAsync(value);

            return Created($"smsapi/{value.SmsApiId}", value);
        }

        // PUT api/SmsApi
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] SmsApi value, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != value.SmsApiId) return BadRequest();

            await _smsapiRepository.UpdateAsync(value);

            return Ok(value);
        }

        // DELETE api/SmsApi
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var smsapi = _smsapiRepository.DeleteAsync(id.ToString());

            return Ok(smsapi);
        }
    }
}