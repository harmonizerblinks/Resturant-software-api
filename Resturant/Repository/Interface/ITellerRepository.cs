using Resturant.Models;
using System.Linq;

namespace Resturant.Repository
{
    public interface ITellerRepository : IGenericRepository<Teller>
    {
        IQueryable<Teller> GetAll();
    }
}
