using cafedebug.backend.application.Audience.Interfaces;
using System.Net;
using System.Net.Mail;

namespace cafedebug.backend.application.Audience.Services;

public class SmtpEmailSenderService : IEmailSenderService
{
    public void SendEmail(MailMessage message)
    {
        using var client = new SmtpClient();
        client.Port = Convert.ToInt16(Environment.GetEnvironmentVariable("SMTP_PORT"));
        client.Host = Environment.GetEnvironmentVariable("SMTP_SERVER");
        client.EnableSsl = true;
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(
            Environment.GetEnvironmentVariable("SMTP_USERNAME"),
            Environment.GetEnvironmentVariable("SMTP_PASSWORD"));
        client.Send(message);
    }
}