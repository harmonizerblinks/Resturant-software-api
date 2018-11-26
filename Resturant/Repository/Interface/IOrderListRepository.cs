using Resturant.Models;
using System.Linq;

namespace Resturant.Repository
{
    public interface IOrderListRepository : IGenericRepository<OrderList>
    {
        IQueryable<OrderList> GetAll();
    }
}
