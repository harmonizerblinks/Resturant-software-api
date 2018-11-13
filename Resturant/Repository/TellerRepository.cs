using Resturant.Models;

namespace Resturant.Repository
{
    public class TellerRepository : GenericRepository<Teller>, ITellerRepository
    {
        public TellerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
