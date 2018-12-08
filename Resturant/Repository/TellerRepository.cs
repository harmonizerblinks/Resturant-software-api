using Microsoft.EntityFrameworkCore;
using Resturant.Models;
using System.Linq;

namespace Resturant.Repository
{
    public class TellerRepository : GenericRepository<Teller>, ITellerRepository
    {
        public TellerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Teller> GetAll()
        {
            return _dbContext.Teller.Include(x => x.Transactions).Include(x => x.Nominal).Include(x => x.AppUser).AsQueryable();
        }
    }
}
