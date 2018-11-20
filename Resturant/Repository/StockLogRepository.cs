using Resturant.Models;

namespace Resturant.Repository
{
    public class StockLogRepository : GenericRepository<StockLog>, IStockLogRepository
    {
        public StockLogRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
