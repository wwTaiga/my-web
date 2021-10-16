using System;

namespace MyWeb.Dtos
{
    public record LoginUserDto(Guid Id, string Username, string Password);

    public record AddLoginUserDto(string Username, string Password);

    public record UpdateLoginUserDto(string Username, string Password);
}
