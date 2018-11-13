using Resturant.Models;

namespace Resturant.Repository
{
    public class SequenceRepository : GenericRepository<Sequence>, ISequenceRepository
    {
        public SequenceRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
