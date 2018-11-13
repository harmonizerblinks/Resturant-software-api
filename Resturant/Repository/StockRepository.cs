using Resturant.Models;

namespace Resturant.Repository
{
    public class StockRepository : GenericRepository<Stock>, IStockRepository
    {
        public StockRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
