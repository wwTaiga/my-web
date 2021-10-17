using Microsoft.AspNetCore.Identity;
using MyWeb.Dtos;

namespace MyWeb.Models
{
    public class LoginUser : IdentityUser
    {
        public LoginUserDto asDto()
        {
            return new LoginUserDto(this.Id, this.UserName);
        }
    }
}
