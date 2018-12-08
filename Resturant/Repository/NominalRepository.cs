using Microsoft.EntityFrameworkCore;
using Resturant.Models;
using System.Linq;

namespace Resturant.Repository
{
    public class NominalRepository : GenericRepository<Nominal>, INominalRepository
    {
        public NominalRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Nominal> GetAll()
        {
            return _dbContext.Nominal.Include(x => x.Transactions).AsQueryable();
        }
    }
}
