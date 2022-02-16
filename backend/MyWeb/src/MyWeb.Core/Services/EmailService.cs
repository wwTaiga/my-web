using System.Threading.Tasks;
using System.Web;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;
using MyWeb.Core.Models;
using MyWeb.Core.Models.Entities;
using MyWeb.Core.Models.Settings;

namespace MyWeb.Core.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly FrontendSettings _frontendSettings;
    private readonly UserManager<LoginUser> _userManager;

    public EmailService(IOptions<EmailSettings> emailSettings,
            IOptions<FrontendSettings> frontendSettings,
            UserManager<LoginUser> userManager)
    {
        _emailSettings = emailSettings.Value;
        _frontendSettings = frontendSettings.Value;
        _userManager = userManager;
    }

    public async Task sendEmailAsync(EmailMessage message)
    {
        var email = emailMessagetoMimeMessage(_emailSettings.From, message);
        using var smtp = new SmtpClient();
        smtp.Connect(_emailSettings.SmtpServer, _emailSettings.SmtpPort,
                SecureSocketOptions.StartTls);
        smtp.Authenticate(_emailSettings.Username, _emailSettings.Password);
        await smtp.SendAsync(email);
        smtp.Disconnect(true);
    }

    private MimeMessage emailMessagetoMimeMessage(string from, EmailMessage message)
    {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(from);
        email.To.AddRange(message.To);
        email.Subject = message.Subject;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };
        return email;
    }

    public async Task sendEmailConfirmationEmailAsync(string confirmationLink, LoginUser user)
    {
        EmailMessage message = new(
            // new string[] { user.Email },
            new string[] { "olgamelv98@gmail.com" },
            "Confirm your account",
            "Please confirm your account by clicking this link: " +
                "<a href=\"" + confirmationLink + "\">link</a>");

        await sendEmailAsync(message);
    }

    public async Task sendResetPasswordEmailAsync(LoginUser user)
    {
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        token = HttpUtility.UrlEncode(token);

        EmailMessage message = new(
            // new string[] { user.Email },
            new string[] { "olgamelv98@gmail.com" },
            "Your Password Reset Link",
            "Please click this link to reset your account password: " +
                "<a href=\"" + _frontendSettings.ResetPasswordUrl(token, user.Id) +
                "\">link</a>");

        await sendEmailAsync(message);
    }
}
