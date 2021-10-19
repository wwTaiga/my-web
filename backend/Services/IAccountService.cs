namespace MyWeb.Services
{
    public interface IAccountService
    {
        /// <summary>
        /// Generate JWT token and return token string
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <returns>
        /// Return token string
        /// </returns>
        string GenerateJwtToken(string userName);
    }
}
