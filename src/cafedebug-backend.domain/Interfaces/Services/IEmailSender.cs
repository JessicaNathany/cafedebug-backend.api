using System.Net.Mail;

namespace cafedebug_backend.domain.Interfaces.Services
{
    public interface IEmailSender
    {
        void SendEmail(MailMessage message);
    }
}
