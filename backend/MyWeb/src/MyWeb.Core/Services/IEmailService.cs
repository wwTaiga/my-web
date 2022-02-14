using System.Threading.Tasks;
using MyWeb.Core.Models;
using MyWeb.Core.Models.Entities;

namespace MyWeb.Core.Services
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

        /// <summary>
        /// Send reset password email.
        /// </summary>
        Task sendResetPasswordEmailAsync(LoginUser user);
    }
}
