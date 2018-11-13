using Resturant.Models;

namespace Resturant.Repository
{
    public class FoodRepository : GenericRepository<Food>, IFoodRepository
    {
        public FoodRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
