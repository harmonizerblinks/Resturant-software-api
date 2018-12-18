using Resturant.Models;
using System.Threading.Tasks;

namespace Resturant.Repository
{
    public interface ISequenceRepository : IGenericRepository<Sequence>
    {
        Task<string> GetCode(string type);
    }
}
