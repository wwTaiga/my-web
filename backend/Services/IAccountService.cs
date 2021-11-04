using MyWeb.Models.Entities;

namespace MyWeb.Services
{
    public interface IAccountService
    {
        /// <summary>
        /// Generate JWT token and return token string
        /// </summary>
        /// <param name="loginUser">Login User</param>
        /// <returns>
        /// Return token string
        /// </returns>
        string GenerateJwtToken(LoginUser loginUser);
    }
}
