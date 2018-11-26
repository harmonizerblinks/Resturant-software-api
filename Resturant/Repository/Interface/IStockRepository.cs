using Resturant.Models;
using System.Linq;

namespace Resturant.Repository
{
    public interface IStockRepository : IGenericRepository<Stock>
    {
        IQueryable<Stock> GetAll();
    }
}
