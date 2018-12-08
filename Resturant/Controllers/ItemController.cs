using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resturant.Repository;
using Resturant.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Resturant.Services;

namespace Resturant.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        //private MyServices _get;
        //private readonly SequenceController _get;
        private readonly IItemRepository _itemRepository;
        private readonly IStockRepository _stockRepository;
        private readonly ISequenceRepository _sequenceRepository;

        public ItemController(IItemRepository itemRepository, IStockRepository stockRepository/*, MyServices get*/)
        {
            //_get = new MyServices();
            //_get = new SequenceController(_sequenceRepository);
            _itemRepository = itemRepository;
            _stockRepository = stockRepository;
        }

        // GET Item
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var item = _itemRepository.Query().Include(s=>s.Stock);

            return Ok(item);
        }

        // GET Item
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var item = _itemRepository.GetAsync(id);

            if (item != null)
            {
                return Ok(item.Result);
            }
            else
                return BadRequest();
        }

        // POST Item
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Item value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            //value.Code = await _get.GetCode("Item");

            await _itemRepository.InsertAsync(value);

            var stock = new Stock() { ItemId = value.ItemId, Quantity = 0, MUserId = value.MUserId, Date = DateTime.UtcNow };
            await _stockRepository.InsertAsync(stock);
            return Created($"item/{value.ItemId}", value);
        }

        // PUT Item
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Item value, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != value.ItemId) return BadRequest();

            //if (value.Code == null) value.Code = await _get.GetCode("Item");

            await _itemRepository.UpdateAsync(value);

            return Ok(value);
        }

        // DELETE Item
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var item = _itemRepository.DeleteAsync(id);

            return Ok(item.Result);
        }
    }
}