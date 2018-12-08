using Resturant.Models;
using System.Linq;

namespace Resturant.Repository
{
    public interface ILocationRepository : IGenericRepository<Location>
    {
        IQueryable<Location> GetAll();
    }
}
