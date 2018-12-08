using Resturant.Models;
using System.Linq;

namespace Resturant.Repository
{
    public interface IDiscountRepository : IGenericRepository<Discount>
    {
        IQueryable<Discount> GetAll();
    }
}
