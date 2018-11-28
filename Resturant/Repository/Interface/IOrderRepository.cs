using Resturant.Models;
using System.Linq;

namespace Resturant.Repository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        IQueryable<Order> GetAll();

        IQueryable<Order> GetTodayOrders();
    }
}
