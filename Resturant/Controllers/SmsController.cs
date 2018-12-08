using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        private readonly ISmsApiRepository _smsapiRepository;

        public SmsController(ISmsRepository smsRepository, ISmsApiRepository smsapiRepository)
        {
            _smsRepository = smsRepository;
            _smsapiRepository = smsapiRepository;
        }

        // GET Sms
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var sms = _smsRepository.Query().OrderByDescending(d=>d.Date);

            return Ok(sms);
        }

        // GET Sms
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

        // GET Sms
        [HttpGet("Retry/{id}")]
        public async Task<IActionResult> RetrybyId([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var config = _smsapiRepository.Query().LastOrDefault(a => a.Status.ToLower().Contains("active") && a.Default == true);
            var sms = _smsRepository.GetAsync(id).Result;
            var httpClient = new HttpClient();
            StringBuilder sb = new StringBuilder();
            sb.Append(config.Url).Append("?&username=").Append(config.Username)
                .Append("&password=").Append(config.Password).Append("&source=").Append(config.SenderId)
                .Append("&destination=").Append(sms.Mobile).Append("&message=").Append(sms.Message);
            var json = await httpClient.GetStringAsync(sb.ToString());
            var smsresponse = JsonConvert.DeserializeObject<SmsResponse>(json);
            sms.Code = smsresponse.Code; sms.Response = smsresponse.Message;

            await _smsRepository.UpdateAsync(sms);

            return Ok(sms);
        }

        // POST Sms
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var config =_smsapiRepository.Query().LastOrDefault(a => a.Status.ToLower().Contains("active") && a.Default == true);
            var sms = new Sms();
            sms.UserId = value.UserId; sms.Date = DateTime.UtcNow; sms.Mobile = value.Mobile;
            sms.Message = $"Welcome to {config.Name} {value.FullName}, your OrderNo: {value.OrderNo}, Amount: {value.Price}, VAT: {value.Vat}, Total: {value.Total}. We are Glad to serve you.";
            
            try
            {
                var httpClient = new HttpClient();
                StringBuilder sb = new StringBuilder();
                sb.Append(config.Url).Append("?&username=").Append(config.Username)
                    .Append("&password=").Append(config.Password).Append("&source=").Append(config.SenderId)
                    .Append("&destination=").Append(sms.Mobile).Append("&message=").Append(sms.Message);
                var json = await httpClient.GetStringAsync(sb.ToString());
                var smsresponse = JsonConvert.DeserializeObject<SmsResponse>(json);
                sms.Code = smsresponse.Code; sms.Response = smsresponse.Message;
                sms.MDate = DateTime.UtcNow;

                await _smsRepository.InsertAsync(sms);
            }
            catch (Exception e)
            {
                sms.Code = 400; sms.Response = "Unable to send sms";
                await _smsRepository.InsertAsync(sms);
                BackgroundJob.Schedule(() => RetrybyId(sms.SmsId), TimeSpan.FromMinutes(3));

            return null;
            }
         return Created($"sms/{sms.SmsId}", sms);
        }

        // PUT Sms
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Sms value, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != value.SmsId) return BadRequest();

            await _smsRepository.UpdateAsync(value);

            return Ok(value);
        }

        // DELETE Sms
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var sms = _smsRepository.DeleteAsync(id);

            return Ok(sms.Result);
        }
    }
}