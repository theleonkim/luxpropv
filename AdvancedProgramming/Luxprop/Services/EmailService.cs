using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Luxprop.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendPasswordResetEmail(string toEmail, string token)
        {
            string host = _config["Smtp:Host"]!;
            int port = int.Parse(_config["Smtp:Port"]!);
            string user = _config["Smtp:User"]!;
            string pass = _config["Smtp:Pass"]!;
            string from = _config["Smtp:From"]!;

            var smtp = new SmtpClient(host)
            {
                Port = port,
                Credentials = new NetworkCredential(user, pass),
                EnableSsl = true
            };

            string subject = "Password Reset Token - 2CRRE Docs";

            string body = $@"
                <h2>Password Reset</h2>
                <p>You requested to reset your password.</p>
                <p><strong>Your recovery token is:</strong></p>
                <h3 style='color:#0d505a'>{token}</h3>
                <br />
                <p>If you did not request this, simply ignore this email.</p>
            ";

            var message = new MailMessage
            {
                From = new MailAddress(user, "Luxprop Notifier"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(toEmail);

            await smtp.SendMailAsync(message);
        }
    }
}
