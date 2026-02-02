using cafedebug.backend.application.Audience.Interfaces;
using System.Net;
using System.Net.Mail;

namespace cafedebug.backend.application.Audience.Services;

public class SmtpEmailSenderService : IEmailSenderService
{
    public void SendEmail(MailMessage message)
    {
        using (var client = new SmtpClient
               {
                   Port = Convert.ToInt16(Environment.GetEnvironmentVariable("SMTP_PORT")),
                   Host = Environment.GetEnvironmentVariable("SMTP_SERVER"),
                   EnableSsl = true,
                   UseDefaultCredentials = false,
                   Credentials = new NetworkCredential(
                       Environment.GetEnvironmentVariable("SMTP_USERNAME"),
                       Environment.GetEnvironmentVariable("SMTP_PASSWORD"))
               })
        {
            client.Send(message);
        }
    }
}