using Microsoft.AspNetCore.Identity;
using MyWeb.Models.Dtos;

namespace MyWeb.Models.Entities
{
    public class LoginUser : IdentityUser
    {
        public LoginUserDto asDto()
        {
            return new LoginUserDto(this.Id, this.UserName);
        }
    }
}
