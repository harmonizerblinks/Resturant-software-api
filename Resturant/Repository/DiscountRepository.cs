using Microsoft.EntityFrameworkCore;
using Resturant.Models;
using System.Linq;

namespace Resturant.Repository
{
    public class DiscountRepository : GenericRepository<Discount>, IDiscountRepository
    {
        public DiscountRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Discount> GetAll()
        {
            return _dbContext.Discount.Include(x => x.Location).AsQueryable();
        }
    }
}
