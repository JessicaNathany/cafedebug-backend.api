using cafedebug_backend.domain.Request;
using System.Net.Mail;

namespace cafedebug_backend.domain.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmail(SendEmailRequest emailRequest);
        Task<MailMessage> ConfigureEmailRecoveryPasswordAsync(SendEmailRequest emailRequest);

        Task<MailMessage> ConfigureEmailAsync(SendEmailRequest emailRequest);
    }
}
