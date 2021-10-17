namespace MyWeb.Dtos
{
    public record LoginDto(string username, string password);

    public record RegisterDto(string username, string password, string email);
}
