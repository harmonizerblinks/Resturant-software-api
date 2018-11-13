using Resturant.Models;

namespace Resturant.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
