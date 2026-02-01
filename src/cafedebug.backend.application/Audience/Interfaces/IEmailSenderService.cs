using System.Net.Mail;

namespace cafedebug.backend.application.Audience.Interfaces;

public interface IEmailSenderService
{
    void SendEmail(MailMessage message);
}