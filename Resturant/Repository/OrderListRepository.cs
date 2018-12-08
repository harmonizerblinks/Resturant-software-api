using Microsoft.EntityFrameworkCore;
using Resturant.Models;
using System.Linq;

namespace Resturant.Repository
{
    public class OrderListRepository : GenericRepository<OrderList>, IOrderListRepository
    {
        public OrderListRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<OrderList> GetAll()
        {
            return _dbContext.OrderList.Include(x => x.Food).AsQueryable();
        }
        
        public IQueryable<OrderList> GetTodayOrder()
        {
            return _dbContext.OrderList.Include(x => x.Food).AsQueryable();
        }
    }
}
