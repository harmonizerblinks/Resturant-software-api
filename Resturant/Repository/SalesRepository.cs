using Resturant.Models;

namespace Resturant.Repository
{
    public class SalesRepository : GenericRepository<Sales>, ISalesRepository
    {
        public SalesRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
