using Resturant.Models;

namespace Resturant.Repository
{
    public class NominalRepository : GenericRepository<Nominal>, INominalRepository
    {
        public NominalRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
