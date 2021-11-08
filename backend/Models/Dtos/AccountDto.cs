using System.ComponentModel.DataAnnotations;

namespace MyWeb.Models.Dtos
{
    /// <summary>
    /// Use for register a new user
    /// </summary>
    /// <param name="userName">User Name</param>
    /// <param name="password">Password</param>
    /// <param name="email">Email</param>
    public record RegisterDto(
        [Required] string userName,
        [Required] string password,
        [Required] string email);
}
