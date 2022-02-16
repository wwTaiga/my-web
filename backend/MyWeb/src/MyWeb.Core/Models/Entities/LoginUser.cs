using Microsoft.AspNetCore.Identity;
using MyWeb.Core.Models.Dtos;

namespace MyWeb.Core.Models.Entities;

public class LoginUser : IdentityUser
{
    public LoginUserDto asDto()
    {
        return new LoginUserDto(this.Id, this.UserName);
    }
}
