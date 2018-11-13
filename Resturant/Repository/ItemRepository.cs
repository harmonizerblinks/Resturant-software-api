using Resturant.Models;

namespace Resturant.Repository
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
