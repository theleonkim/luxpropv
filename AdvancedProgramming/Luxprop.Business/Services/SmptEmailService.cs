using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default);
}

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _cfg;

    public SmtpEmailService(IConfiguration cfg) => _cfg = cfg;

    public async Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default)
    {
        using var msg = new MailMessage();

        // Gmail requiere que el "From" sea el mismo correo autenticado
        var fromAddress = _cfg["Smtp:User"] ?? _cfg["Smtp:From"];
        msg.From = new MailAddress(fromAddress!, "Luxprop Notifier");

        msg.To.Add(to);
        msg.Subject = subject;
        msg.Body = htmlBody;
        msg.IsBodyHtml = true;

        using var client = new SmtpClient(_cfg["Smtp:Host"])
        {
            Port = int.Parse(_cfg["Smtp:Port"] ?? "587"),
            Credentials = new NetworkCredential(_cfg["Smtp:User"], _cfg["Smtp:Pass"]),
            EnableSsl = true // Gmail usa STARTTLS (puerto 587)
        };

        try
        {
            await client.SendMailAsync(msg, ct);
        }
        catch (SmtpException ex)
        {
            // Log opcional
            Console.WriteLine($"Error enviando correo: {ex.Message}");
            throw; // Re-lanza la excepción para que Blazor o el servicio la manejen
        }
    }
}
