using cafedebug_backend.domain.Interfaces.Services;
using System.Net;
using System.Net.Mail;

namespace cafedebug.backend.application.Service;

public class SmtpEmailSender : IEmailSender
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
                       Environment.GetEnvironmentVariable("SMTP_FROM"),
                       Environment.GetEnvironmentVariable("SMTP_PASSWORD"))
               })
        {
            client.Send(message);
        }
    }
}