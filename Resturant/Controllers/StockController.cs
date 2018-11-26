using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resturant.Repository;
using Resturant.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Resturant.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;
        private readonly IStockLogRepository _stocklogRepository;
        private readonly IAppUserRepository _appuserRepository;

        public StockController(IStockRepository stockRepository, IStockLogRepository stocklogRepository,
            IAppUserRepository appuserRepository)
        {
            _stockRepository = stockRepository;
            _stocklogRepository = stocklogRepository;
            _appuserRepository = appuserRepository;
        }

        // GET api/Stock
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var stock = _stockRepository.GetAll();

            return Ok(stock);
        }

        // GET api/Stock
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var stock = _stockRepository.GetAsync(id);

            if (stock != null)
            {
                return Ok(stock.Result);
            }
            else
                return BadRequest();
        }


        // GET api/Stock/log/id
        [HttpGet("log/{id}")]
        public async Task<IActionResult> GetLogById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            //var log = _stocklogRepository.GetAll().Where(l => l.ItemId == id).OrderByDescending(o => o.Date).ToList();
            var log = _stocklogRepository.GetAll().Select(
                    u => new StockLog()
                    {
                        StockId = u.StockId, Name = u.Item.Name, Date = u.Date,
                        Price = u.Quantity * u.Item.Price, Quantity = u.Quantity,
                        UserId = _appuserRepository.Query().Where(i => i.Id == u.UserId).Select(n => n.UserName).FirstOrDefault()
                    }).OrderByDescending(o => o.Date).ToList();

            if (log.Count >= 0)
            {
                return Ok(log);
            }
            else
                return BadRequest();
        }

        // GET api/Stock/log
        [HttpGet("log")]
        public async Task<IActionResult> GetLog()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            // var log = _stocklogRepository.GetAll().OrderByDescending(o => o.Date).ToList();
            var log = _stocklogRepository.GetAll().Select(
                    u => new StockLog()
                    {
                        StockId = u.StockId, Name = u.Item.Name, Date = u.Date,
                        Price = u.Quantity * u.Item.Price, Quantity = u.Quantity,
                        UserId = _appuserRepository.Query().Where(i => i.Id == u.UserId).Select(n => n.UserName.ToLower()).FirstOrDefault()
                    }).OrderByDescending(o => o.Date).ToList();

            if (log.Count >= 0)
            {
                return Ok(log);
            }
            else
                return BadRequest();
        }

        // POST api/Stock
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Stock value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var log = new StockLog();
            log.ItemId = value.ItemId; log.Quantity = value.Quantity;
            log.UserId = value.UserId; log.Date = value.Date;
            var stock = _stockRepository.Query().Where(s=>s.ItemId == value.ItemId).FirstOrDefault();
            if (stock != null)
            {
                stock.Quantity += value.Quantity; log.StockId = stock.StockId;
                stock.MUserId = value.MUserId; stock.MDate = value.MDate;
                await _stockRepository.UpdateAsync(stock);
                await _stocklogRepository.InsertAsync(log);
                return Ok(stock);
            }
            await _stockRepository.InsertAsync(value);
            log.StockId = stock.StockId;
            await _stocklogRepository.InsertAsync(log);

            return Created($"stock/{value.StockId}", value);
        }

        // PUT api/Stock
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Stock value, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != value.StockId) return BadRequest();

            await _stockRepository.UpdateAsync(value);

            return Ok(value);
        }

        // DELETE api/Stock
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var stock = _stockRepository.DeleteAsync(id);

            return Ok(stock.Result);
        }
    }
}