using System.Threading.Tasks;

namespace Resturant.Services
{
    public interface IEmailSender
    {
       Task SendEmailAsync(string email, string subject, string message);
    }
}