using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Ordering.Application.Common.Infrastructure;
using Ordering.Infrastructure.Extensions;

namespace Ordering.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = GetMailMessage(to, subject, body);
            if (message is not null)
            {
                using var client = new SmtpClient();
                await client.ConnectAsync(AppSettings.EmailSettings.Host, AppSettings.EmailSettings.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(AppSettings.EmailSettings.UserName, AppSettings.EmailSettings.MailPassword);
                var response = await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        private MimeMessage GetMailMessage(string userEmail, string subject, string body)
        {
            var message = new MimeMessage
            {
                Subject = subject
            };

            message.From.Add(new MailboxAddress(AppSettings.EmailSettings.Name, AppSettings.EmailSettings.EmailFrom));
            message.To.Add(MailboxAddress.Parse(userEmail));

            var builder = new BodyBuilder
            {
                HtmlBody = body.Replace("\n", "<br>")
            };

            message.Body = builder.ToMessageBody();

            return message;
        }
    }
}
