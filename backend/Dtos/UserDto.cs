using System;

namespace MyWeb.Dtos
{
    public record UserDto(Guid Id, string Username, string Password);

    public record CreateUserDto(string Username, string Password);

    public record UpdateUserDto(string Username, string Password);
}
