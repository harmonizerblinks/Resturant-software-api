using Microsoft.EntityFrameworkCore;
using Resturant.Models;
using System.Linq;

namespace Resturant.Repository
{
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        public LocationRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Location> GetAll()
        {
            return _dbContext.Location.Include(x => x.Discounts).AsQueryable();
        }
    }
}
