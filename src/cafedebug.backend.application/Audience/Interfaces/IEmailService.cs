using System.Net.Mail;
using cafedebug_backend.domain.Messages.Email.Request;

namespace cafedebug.backend.application.Audience.Interfaces;

public interface IEmailService
{
    Task SendEmail(SendEmailRequest emailRequest);
    Task<MailMessage> ConfigureEmailRecoveryPasswordAsync(SendEmailRequest emailRequest);
    Task<MailMessage> ConfigureEmailAsync(SendEmailRequest emailRequest);
}