using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Repository
{
    public interface IGenericRepository<T>
    {
        Task<T> GetAsync(string id);

        IQueryable<T> Query();

        Task InsertAsync(T entity);
        Task InsertRangeAsync(IEnumerable<T> entities);

        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entities);

        Task<T> DeleteAsync(string id);
        Task DeleteRangeAsync(IEnumerable<T> entities);
    }
}
