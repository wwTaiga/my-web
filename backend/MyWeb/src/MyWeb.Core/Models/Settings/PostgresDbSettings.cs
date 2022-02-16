namespace MyWeb.Core.Models.Settings;

public class PostgresDbSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Database { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string ConnectionString => $"Host={Host};Port={Port};"
        + $"Database={Database};Username={Username};Password={Password}";
}
