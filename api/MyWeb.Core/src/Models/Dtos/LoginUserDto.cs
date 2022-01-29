namespace MyWeb.Models.Dtos
{
    public record LoginUserDto(string Id, string Username);

    public record AddLoginUserDto(string Username, string Password);

    public record UpdateLoginUserDto(string Username, string Password);
}
