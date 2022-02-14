using System.ComponentModel.DataAnnotations;

namespace MyWeb.Core.Models.Dtos
{
    /// <summary>
    /// Use for register a new user.
    /// </summary>
    /// <param name="UserName">User Name</param>
    /// <param name="Password">Password</param>
    /// <param name="Email">Email</param>
    public record RegisterDto(
        [Required] string UserName,
        [Required] string Password,
        [Required] string Email);

    /// <summary>
    /// Use for reset password.
    /// </summary>
    /// <param name="Token">Reset Password Token</param>
    /// <param name="UserId">User Id</param>
    /// <param name="NewPassword">New Password</param>
    public record ResetPasswordDto(
        [Required] string Token,
        [Required] string UserId,
        [Required] string NewPassword);
}
