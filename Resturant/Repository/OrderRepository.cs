using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Resturant.Models;
using System.Linq;

namespace Resturant.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public IQueryable<Order> GetAll()
        {
            return _dbContext.Order.Include(x => x.Orderlist).AsQueryable();
        }
    }
}
