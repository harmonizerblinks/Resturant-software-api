using Microsoft.EntityFrameworkCore;
using Resturant.Models;
using System.Linq;

namespace Resturant.Repository
{
    public class StockRepository : GenericRepository<Stock>, IStockRepository
    {
        public StockRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Stock> GetAll()
        {
            return _dbContext.Stock.Include(x => x.Item).Include(x => x.Logs).AsQueryable();
        }
    }
}
