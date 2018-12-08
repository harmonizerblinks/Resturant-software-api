using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Resturant.Models;
using Resturant.Repository;

namespace Resturant.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class TellerController : ControllerBase
    {
        private readonly ITellerRepository _tellerRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly INominalRepository _nominalRepository;

        public TellerController(ITellerRepository tellerRepository, ITransactionRepository transactionRepository,
            INominalRepository nominalRepository)
        {
            _tellerRepository = tellerRepository;
            _nominalRepository = nominalRepository;
            _transactionRepository = transactionRepository;
        }

        // GET Teller
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var teller = _tellerRepository.GetAll().Select(t => new {
                t.TellerId,
                t.Nominal.Code, t.Id,
                Name = t.Nominal.Description,
                User = t.AppUser.UserName,
                NoOfTrans = t.Transactions.Where(c => c.UserId == t.Id).Count() }).ToList();

            return Ok(teller);
        }

        // GET Teller
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var teller = _tellerRepository.GetAsync(id);

            if (teller != null)
            {
                return Ok(teller.Result);
            }
            else
                return BadRequest();
        }
        
        // GET Order/Balance/userid
        [HttpGet("Balance/{id}")]
        public async Task<IActionResult> GetTellerBalance([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var teller = _tellerRepository.GetAll().Where(t => t.TellerId == id).FirstOrDefault();
            //.Select(t => t.Transactions.Where(c => c.Type == "Credit").Select(a => a.Amount).Sum()
            //        - t.Transactions.Where(c => c.Type == "Debit").Select(a => a.Amount).Sum());
            decimal bal = 0;
            if (teller.Transactions.Count > 0)
            {
                bal = teller.Transactions.Where(c => c.Type == "Credit" && c.TellerId == teller.TellerId && c.NominalId == teller.NominalId).Select(a => a.Amount).Sum()
                          - teller.Transactions.Where(c => c.Type == "Debit" && c.TellerId == teller.TellerId && c.NominalId == teller.NominalId).Select(a => a.Amount).Sum();
            }

            return Ok(bal);
        }

        
        // POST Teller
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Teller value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var app = _tellerRepository.Query().Any(u => u.Id.Equals(value.Id));
            if (app) return BadRequest("User is Already a Valid Teller");

            await _tellerRepository.InsertAsync(value);

            var teller = _tellerRepository.GetAll().Where(i=>i.Id == value.Id).Select(t => new {
                t.Id,
                t.Nominal.Code,
                Name = t.Nominal.Description,
                User = t.AppUser.UserName,
                NoOfTrans = t.Transactions.Where(c => c.UserId == t.Id).Count()
            }).ToList();

            return Created($"teller/{value.Id}", teller);
        }

        // POST Teller/transfer
        [HttpPost("Transfer")]
        public async Task<IActionResult> PostTransfer([FromBody] Transaction value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var from = _tellerRepository.Query().Where(i => i.Id == value.UserId).FirstOrDefault();
            if (from == null) return BadRequest("You Are not allowed to Make Transfer");

            var to = _tellerRepository.Query().Where(i => i.TellerId == value.TellerId).FirstOrDefault();
            if (to == null) return BadRequest($"There is no valid teller with Id {value.TellerId}");
            value.NominalId = to.NominalId; value.Type = "Credit"; value.Source = "Teller Transfer";
            var tell = new Transaction()
            {
                TransCode = value.TransCode, Amount = value.Amount,
                Method = value.Method, Source = "Teller Transfer", Type = "Debit",
                NominalId = from.NominalId, TellerId = from.TellerId,
                Reference = value.Reference, UserId = value.UserId, Date = value.Date
            };
            await _transactionRepository.InsertAsync(value);
            await _transactionRepository.InsertAsync(tell);

            return Created($"transaction/{value.TransactionId}", value);
        }

        // POST Teller/voucher
        [HttpPost("Voucher")]
        public async Task<IActionResult> PostVoucher([FromBody] Transaction value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var from = _tellerRepository.Query().Where(i => i.Id == value.UserId).FirstOrDefault();
            if (from == null) return BadRequest("You Are not allowed to Make Transfer");

            var to = _nominalRepository.Query().Where(i => i.NominalId == value.NominalId).FirstOrDefault();
            if (to == null) return BadRequest($"There is no Nominal with Id {value.NominalId}");
            value.NominalId = to.NominalId; value.Type = "Credit"; value.Source = "Teller Voucher";
            var tell = new Transaction()
            {
                TransCode = value.TransCode, Amount = value.Amount, Method = value.Method,
                Source = "Teller Voucher", Type = "Debit", NominalId = from.NominalId,
                TellerId = from.TellerId, Reference = value.Reference, UserId = value.UserId, Date = value.Date
            };
            await _transactionRepository.InsertAsync(value);
            await _transactionRepository.InsertAsync(tell);

            return Created($"transaction/{value.TransactionId}", value);
        }

        // PUT Teller
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Teller value, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != value.TellerId) return BadRequest();

            await _tellerRepository.UpdateAsync(value);

            return Ok(value);
        }

        // DELETE Teller
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var teller = _tellerRepository.DeleteAsync(id);

            return Ok(teller.Result);
        }
    }
}