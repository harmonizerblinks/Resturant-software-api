using Resturant.Models;

namespace Resturant.Repository
{
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        public LocationRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
