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
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly INominalRepository _nominalRepository;

        public TransactionController(ITransactionRepository transactionRepository, INominalRepository nominalRepository)
        {
            _transactionRepository = transactionRepository;
            _nominalRepository = nominalRepository;
        }

        // GET Transaction
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var transaction = _transactionRepository.Query();

            return Ok(transaction);
        }

        // GET Transaction
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var transaction = _transactionRepository.GetAsync(id);

            if (transaction != null)
            {
                return Ok(transaction.Result);
            }
            else
                return BadRequest();
        }

        // POST Transaction
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Transaction value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var from = _nominalRepository.Query().Where(i => i.NominalId == value.Id).FirstOrDefault();
            if (from == null) return BadRequest("Select a Valid Funding Source");

            var to = _nominalRepository.Query().Where(i => i.NominalId == value.NominalId).FirstOrDefault();
            if (to == null) return BadRequest($"Select a Valid Expense Code Nominal Code");
            value.NominalId = to.NominalId; value.Type = "Credit"; value.Source = "Teller Voucher";
            var tell = new Transaction()
            {
                TransCode = value.TransCode, Amount = value.Amount,
                Method = value.Method, Source = "Teller Voucher",
                Type = "Debit", NominalId = from.NominalId,
                Reference = value.Reference, UserId = value.UserId, Date = value.Date
            };
            await _transactionRepository.InsertAsync(value);
            await _transactionRepository.InsertAsync(tell);

            return Created($"transaction/{value.TransactionId}", value);
        }

        // PUT Transaction/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Transaction value, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != value.TransactionId) return BadRequest();

            await _transactionRepository.UpdateAsync(value);

            return Ok(value);
        }

        // DELETE Transaction
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var transaction = _transactionRepository.DeleteAsync(id);

            return Ok(transaction.Result);
        }

    }
}