using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MyWeb.Models;
using MyWeb.Models.Entities;
using MyWeb.Models.Settings;

namespace MyWeb.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
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
            email.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
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
    }
}
