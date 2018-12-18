using Resturant.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Repository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        IQueryable<Order> GetAll();

        IQueryable<Order> GetTodayOrders(DateTime date);

        Task<Dashboard> GetDashboard();
    }
}
