using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resturant.Models;
using Resturant.Repository;

namespace Resturant.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionController(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        // GET api/Transaction
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var transaction = _transactionRepository.Query();

            return Ok(transaction);
        }

        // GET api/Transaction
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var transaction = _transactionRepository.GetAsync(id.ToString());

            if (transaction != null)
            {
                return Ok(transaction);
            }
            else
                return BadRequest();
        }

        // POST api/Transaction
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Transaction value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _transactionRepository.InsertAsync(value);

            return Created($"transaction/{value.TransactionId}", value);
        }

        // PUT api/Transaction/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Transaction value, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != value.TransactionId) return BadRequest();

            await _transactionRepository.UpdateAsync(value);

            return Ok(value);
        }

        // DELETE api/Transaction
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var transaction = _transactionRepository.DeleteAsync(id.ToString());

            return Ok(transaction);
        }

    }
}