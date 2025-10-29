using System.Net.Mail;

namespace cafedebug_backend.domain.Messages.Email.Services;

public interface IEmailSender
{
    void SendEmail(MailMessage message);
}