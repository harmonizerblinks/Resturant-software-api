using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Resturant.Models;
using System;
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

        public IQueryable<Order> GetTodayOrders(DateTime date)
        {
            return _dbContext.Order.Where(d=>d.Date.Date == date.Date ).Include(x => x.Orderlist).AsQueryable();
        }
    }
}
