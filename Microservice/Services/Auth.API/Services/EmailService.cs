using Auth.API.Infrastructure;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.OpenApi.Extensions;
using MimeKit;
using Shared.API.Enums;

namespace Auth.API.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(Guid id, string userEmail, string token, EmailSubject subject);
    }

    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(Guid id, string userEmail, string token, EmailSubject subject)
        {
            var message = GetMailMessage(id, userEmail, token, subject);
            if (message is not null)
            {
                using var client = new SmtpClient();
                await client.ConnectAsync(AppSettings.EmailSettings.Host, AppSettings.EmailSettings.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(AppSettings.EmailSettings.UserName, AppSettings.EmailSettings.MailPassword);
                var response = await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        private MimeMessage GetMailMessage(Guid id, string userEmail, string token, EmailSubject subject)
        {
            var message = new MimeMessage
            {
                Subject = subject switch
                {
                    EmailSubject.EmailConfirmation => EnumExtensions.GetDisplayName(EmailSubject.EmailConfirmation),
                    EmailSubject.ResetPasswordEmail => EnumExtensions.GetDisplayName(EmailSubject.ResetPasswordEmail),
                    _ => "System Error"
                }
            };

            message.From.Add(new MailboxAddress(AppSettings.EmailSettings.Name, AppSettings.EmailSettings.EmailFrom));
            message.To.Add(MailboxAddress.Parse(userEmail));

            var uri = string.Empty;
            var content = string.Empty;

            if (subject is EmailSubject.EmailConfirmation)
            {
                uri = $"/gateway/Authentication/confirmEmail/{id}/{token}";
                content = $@"
                    <p>
                        Please <a href='{AppSettings.Settings.GatewayUri + uri}'> click here </a> to confirm your registration!
                    </p>";
            }
            else
            {
                uri = $"/authentication/resetPassword/{id}?token={token}";
                content = $@"
                    <p>
                        Please <a href='{AppSettings.Settings.ClientUri + uri}'> click here </a> to reset your password!
                    </p>";
            }

            var builder = new BodyBuilder
            {
                HtmlBody = content.Replace("\n", "<br>")
            };

            message.Body = builder.ToMessageBody();

            return message;
        }
    }
}
