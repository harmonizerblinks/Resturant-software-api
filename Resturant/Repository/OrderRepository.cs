using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Resturant.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            return _dbContext.Order.Include(x => x.Orderlist).Include(x => x.Location).AsQueryable();
        }

        public IQueryable<Order> GetTodayOrders(DateTime date)
        {
            return _dbContext.Order.Where(d=>d.Date.Date == date.Date )
                .Include(x => x.Orderlist).Include(x => x.Location).AsQueryable();
        }

        public async Task<Dashboard> GetDashboard()
        {
            var dash = new Dashboard
            {
                Company = _dbContext.Company.LastOrDefault(),
                Order = _dbContext.Order.Count(),
                Orderlist = _dbContext.OrderList.Count(),
                Sale = _dbContext.Sales.Count(),
                Sequence = _dbContext.Sequence.Count(),
                Users = _dbContext.Users.Count(),
                Employee = _dbContext.Employee.Count(),
                Teller = _dbContext.Teller.Count(),
                Transit = _dbContext.Transit.Count(),
                Nominal = _dbContext.Nominal.Count(),
                Transaction = _dbContext.Transaction.Count(),
                Item = _dbContext.Item.Count(),
                Food = _dbContext.Food.Count(),
                Stock = _dbContext.Stock.Count(),
                StockLog = _dbContext.StockLog.Count(),
                Discount = _dbContext.Discount.Count(),
                Location = _dbContext.Location.Count(),
                Sms = _dbContext.Sms.Count(),
                SmsApi = _dbContext.SmsApi.Count(),
            };
            return dash;
        }
    }
}
