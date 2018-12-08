using Microsoft.EntityFrameworkCore;
using Resturant.Models;
using System.Linq;

namespace Resturant.Repository
{
    public class TransitRepository : GenericRepository<Transit>, ITransitRepository
    {
        public TransitRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Transit> GetAll()
        {
            return _dbContext.Transit.Include(x => x.Nominal).AsQueryable();
        }
    }
}
