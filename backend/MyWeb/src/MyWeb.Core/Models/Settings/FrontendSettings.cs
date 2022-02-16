namespace MyWeb.Core.Models.Settings;

public class FrontendSettings
{
    public string DomainName { get; set; }
    public FrontendUrls Url { get; set; }

    public string ResetPasswordUrl(string token, string userId)
    {
        return DomainName + Url.ResetPassword + "?token=" + token + "&userId=" + userId;
    }
}
