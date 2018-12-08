using Resturant.Models;
using System.Linq;

namespace Resturant.Repository
{
    public interface INominalRepository : IGenericRepository<Nominal>
    {
        IQueryable<Nominal> GetAll();
    }
}
