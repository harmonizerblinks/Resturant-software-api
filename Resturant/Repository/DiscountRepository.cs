using Resturant.Models;

namespace Resturant.Repository
{
    public class DiscountRepository : GenericRepository<Discount>, IDiscountRepository
    {
        public DiscountRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
