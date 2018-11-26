using Microsoft.EntityFrameworkCore;
using Resturant.Models;
using System.Linq;

namespace Resturant.Repository
{
    public class SalesRepository : GenericRepository<Sales>, ISalesRepository
    {
        public SalesRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Sales> GetAll()
        {
            return _dbContext.Sales.Include(x => x.Item).AsQueryable();
        }
    }
}
