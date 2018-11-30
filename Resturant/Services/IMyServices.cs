using Resturant.Models;
using System.Threading.Tasks;

namespace Resturant.Services
{
    public interface IMyServices
    {
        Task<string> GetCode(string type);
        
        Task<Sms> Sms(Order order);

        Task<string> RefreshOrder();
    }
}
