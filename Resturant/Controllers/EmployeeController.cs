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
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // GET Employee
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var employee = _employeeRepository.Query();

            return Ok(employee);
        }

        // GET Employee
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var employee = _employeeRepository.GetAsync(id);

            if (employee != null)
            {
                return Ok(employee.Result);
            }
            else
                return BadRequest();
        }

        // POST Employee
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Employee value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _employeeRepository.InsertAsync(value);

            return Created($"employee/{value.EmployeeId}", value);
        }

        // PUT Employee
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Employee value, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != value.EmployeeId) return BadRequest();

            await _employeeRepository.UpdateAsync(value);

            return Ok(value);
        }

        // DELETE Employee
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var employee = _employeeRepository.DeleteAsync(id);

            return Ok(employee.Result);
        }

    }
}