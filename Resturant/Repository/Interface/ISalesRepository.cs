using Resturant.Models;
using System.Linq;

namespace Resturant.Repository
{
    public interface ISalesRepository : IGenericRepository<Sales>
    {
        IQueryable<Sales> GetAll();
    }
}
