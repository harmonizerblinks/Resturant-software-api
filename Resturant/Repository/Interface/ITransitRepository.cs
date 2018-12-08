using Resturant.Models;
using System.Linq;

namespace Resturant.Repository
{
    public interface ITransitRepository : IGenericRepository<Transit>
    {
        IQueryable<Transit> GetAll();
    }
}
