using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Resturant.Models;
using Resturant.Repository;

namespace Resturant.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {

        private readonly ICompanyRepository _companyRepository;

        public CompanyController(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        // GET api/Company
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var company = _companyRepository.Query();

            return Ok(company);
        }

        // GET api/Company
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var company = _companyRepository.GetAsync(id.ToString());

            if (company != null)
            {
                return Ok(company);
            }
            else
                return BadRequest();
        }

        // POST api/Company
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Company value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _companyRepository.InsertAsync(value);

            return Created($"company/{value.CompanyId}", value);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Company value, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != value.CompanyId) return BadRequest();

            await _companyRepository.UpdateAsync(value);

            return Ok(value);
        }


        // DELETE api/Company
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var company = _companyRepository.DeleteAsync(id.ToString());

            return Ok(company);
        }
    }
}
