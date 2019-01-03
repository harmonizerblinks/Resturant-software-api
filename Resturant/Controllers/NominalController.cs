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
    public class NominalController : ControllerBase
    {
        private readonly ITellerRepository _tellerRepository;
        private readonly INominalRepository _nominalRepository;
        private readonly ITransactionRepository _transactionRepository;

        public NominalController(INominalRepository nominalRepository, ITellerRepository tellerRepository, 
            ITransactionRepository transactionRepository)
        {
            _tellerRepository = tellerRepository;
            _nominalRepository = nominalRepository;
            _transactionRepository = transactionRepository;
        }

        // GET Nominal
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var nominal = _nominalRepository.Query();

            return Ok(nominal);
        }

        // GET Nominal
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var nominal = _nominalRepository.GetAsync(id);

            if (nominal != null)
            {
                return Ok(nominal.Result);
            }
            else
                return BadRequest();
        }

        // GET Nominal
        [HttpGet("balancetype/{type}")]
        public async Task<IActionResult> GetByBalanceType([FromRoute] string type)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var nominal = _nominalRepository.Query().Where(b=>b.BalanceType.ToLower() == type.ToLower());

            if (nominal.Count() >= 0)
            {
                return Ok(nominal);
            }
            else
                return BadRequest();
        }

        // GET Nominal
        [HttpGet("gltype/{type}")]
        public async Task<IActionResult> GetByGlType([FromRoute] string type)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var nominal = _nominalRepository.Query().Where(b => b.GLType.ToLower() == type.ToLower());

            if (nominal.Count() >= 0)
            {
                return Ok(nominal);
            }
            else
                return BadRequest();
        }

        // GET Order/Balance/userid
        [HttpGet("Balance/{id}")]
        public async Task<IActionResult> GetTellerBalance([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var teller = _nominalRepository.GetAll().Where(t => t.NominalId == id).FirstOrDefault();
            //.Select(t => t.Transactions.Where(c => c.Type == "Credit").Select(a => a.Amount).Sum()
            //        - t.Transactions.Where(c => c.Type == "Debit").Select(a => a.Amount).Sum());
            decimal bal = 0;
            if (teller.Transactions.Count > 0)
            {
                bal = teller.Transactions.Where(c => c.Type == "Credit" && c.NominalId == teller.NominalId).Select(a => a.Amount).Sum()
                          - teller.Transactions.Where(c => c.Type == "Debit" && c.NominalId == teller.NominalId).Select(a => a.Amount).Sum();
            }

            return Ok(bal);
        }
        
        // POST Nominal
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Nominal value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _nominalRepository.InsertAsync(value);

            return Created($"nominal/{value.NominalId}", value);
        }

        // POST Teller/transfer
        [HttpPost("Transfer")]
        public async Task<IActionResult> PostTransfer([FromBody] Transaction value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var from = _tellerRepository.Query().Where(i => i.TellerId == value.TellerId).FirstOrDefault();
            if (from == null) return BadRequest("You Are not allowed to Make Transfer");

            var to = _nominalRepository.Query().Where(i => i.NominalId == value.NominalId).FirstOrDefault();
            if (to == null) return BadRequest($"There is no valid Nominal with Id {value.NominalId}");
            value.NominalId = to.NominalId; value.Type = "Debit"; value.Source = "Financial Transfer";
            value.TellerId = null;
           var tell = new Transaction()
            {
                TransCode = value.TransCode, Amount = value.Amount,
                Method = value.Method, Source = "Financial Transfer",
                Type = "Credit", NominalId = from.NominalId,
                TellerId = from.TellerId, Reference = value.Reference,
                UserId = value.UserId, Date = value.Date
            };
            await _transactionRepository.InsertAsync(value);
            await _transactionRepository.InsertAsync(tell);

            return Created($"transaction/{value.TransactionId}", value);
        }

        // POST Teller/Payment
        [HttpPost("Payment")]
        public async Task<IActionResult> PostVoucher([FromBody] Transaction value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var from = _tellerRepository.Query().Where(i => i.Id == value.UserId).FirstOrDefault();
            if (from == null) return BadRequest("You Are not allowed to Make Transfer");

            var to = _nominalRepository.Query().Where(i => i.NominalId == value.NominalId).FirstOrDefault();
            if (to == null) return BadRequest($"There is no Nominal with Id {value.NominalId}");
            value.NominalId = to.NominalId; value.Type = "Debit"; value.Source = "Teller Voucher";
            var tell = new Transaction()
            {
                TransCode = value.TransCode,
                Amount = value.Amount,
                Method = value.Method,
                Source = "Teller Voucher",
                Type = "Credit",
                NominalId = from.NominalId,
                TellerId = from.TellerId,
                Reference = value.Reference,
                UserId = value.UserId,
                Date = value.Date
            };
            await _transactionRepository.InsertAsync(value);
            await _transactionRepository.InsertAsync(tell);

            return Created($"transaction/{value.TransactionId}", value);
        }

        // PUT Nominal/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Nominal value, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != value.NominalId) return BadRequest();

            await _nominalRepository.UpdateAsync(value);

            return Ok(value);
        }

        // DELETE Nominal
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var nominal = _nominalRepository.DeleteAsync(id);

            return Ok(nominal.Result);
        }

    }
}