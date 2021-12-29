using System.Threading.Tasks;
using MyWeb.Models;
using MyWeb.Models.Entities;

namespace MyWeb.Services
{
    public interface IEmailService
    {
        /// <summary>
        /// Send Email.
        /// </summary>
        /// <param name="message"><c>EmailMessage</c> object</param>
        Task sendEmailAsync(EmailMessage message);

        /// <summary>
        /// Send confirmation email.
        /// </summary>
        Task sendEmailConfirmationEmailAsync(string confirmationLink, LoginUser user);
    }
}
