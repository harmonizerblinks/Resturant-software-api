using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resturant.Repository;
using Resturant.Models;

namespace Resturant.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;

        public ItemController(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        // GET api/Item
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var item = _itemRepository.Query();

            return Ok(item);
        }

        // GET api/Item
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var item = _itemRepository.GetAsync(id.ToString());

            return Ok(item);
        }

        // POST api/Item
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Item value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _itemRepository.InsertAsync(value);

            return Created($"item/{value.ItemId}", value);
        }

        // PUT api/Item
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Item value, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != value.ItemId) return BadRequest();

            await _itemRepository.UpdateAsync(value);

            return Ok(value);
        }

        // DELETE api/Item
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var item = _itemRepository.DeleteAsync(id.ToString());

            return Ok(item);
        }
    }
}