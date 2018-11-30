using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resturant.Repository;
using Resturant.Models;
using Resturant.Services;

namespace Resturant.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private IMyServices _get;
        private readonly IFoodRepository _foodRepository;

        public FoodController(IFoodRepository foodRepository, IMyServices get)
        {
            _get = get;
            _foodRepository = foodRepository;
        }

        // GET api/Food
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var food = _foodRepository.Query();

            return Ok(food);
        }

        // GET api/Food
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var food = _foodRepository.GetAsync(id);

            if (food != null)
            {
                return Ok(food.Result);
            }
            else
                return BadRequest();
        }

        // POST api/Food
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Food value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            value.Code = await _get.GetCode("Food");
            await _foodRepository.InsertAsync(value);

            return Created($"food/{value.FoodId}", value);
        }

        // PUT api/Food
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Food value, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != value.FoodId) return BadRequest();

            if(value.Code == null) value.Code = await _get.GetCode("Food");

            await _foodRepository.UpdateAsync(value);

            return Ok(value);
        }

        // DELETE api/Food
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var food = _foodRepository.DeleteAsync(id);

            return Ok(food.Result);
        }

    }
}