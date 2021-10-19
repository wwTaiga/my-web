namespace MyWeb.Dtos
{
    public record LoginDto(string userName, string password);

    public record RegisterDto(string userName, string password, string email);
}
