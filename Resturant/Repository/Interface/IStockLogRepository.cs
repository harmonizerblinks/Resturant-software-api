using Resturant.Models;
using System.Linq;

namespace Resturant.Repository
{
    public interface IStockLogRepository : IGenericRepository<StockLog>
    {
        IQueryable<StockLog> GetAll();
    }
}
