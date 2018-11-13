using Resturant.Models;

namespace Resturant.Repository
{
    public class OrderListRepository : GenericRepository<OrderList>, IOrderListRepository
    {
        public OrderListRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
