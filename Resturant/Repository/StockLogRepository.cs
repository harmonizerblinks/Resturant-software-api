using Microsoft.EntityFrameworkCore;
using Resturant.Models;
using System.Linq;

namespace Resturant.Repository
{
    public class StockLogRepository : GenericRepository<StockLog>, IStockLogRepository
    {
        public StockLogRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<StockLog> GetAll()
        {
            return _dbContext.StockLog.Include(x => x.Item).Include(x => x.Stock).AsQueryable();
        }
    }
}
