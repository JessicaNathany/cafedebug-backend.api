using System.Net.Mail;
using cafedebug_backend.domain.Messages.Email.Request;

namespace cafedebug_backend.domain.Messages.Email.Services;

public interface IEmailService
{
    Task SendEmail(SendEmailRequest emailRequest);
    Task<MailMessage> ConfigureEmailRecoveryPasswordAsync(SendEmailRequest emailRequest);

    Task<MailMessage> ConfigureEmailAsync(SendEmailRequest emailRequest);
}