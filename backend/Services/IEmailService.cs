using System.Threading.Tasks;
using MyWeb.Models;
using MyWeb.Models.Entities;

namespace MyWeb.Services
{
    public interface IEmailService
    {
        Task sendEmailAsync(EmailMessage message);

        Task sendEmailConfirmationEmailAsync(string confirmationLink, LoginUser user);
    }
}
