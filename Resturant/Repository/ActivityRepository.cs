using Resturant.Models;

namespace Resturant.Repository
{
    public class ActivityRepository : GenericRepository<Activity>, IActivityRepository
    {
        public ActivityRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
