using cafedebug.backend.application.Constants;
using cafedebug_backend.domain.Interfaces.Services;
using cafedebug_backend.domain.Request;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Text;

namespace cafedebug.backend.application.Service
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IEmailSender _emailSender;
        public EmailService(ILogger<EmailService> logger, IEmailSender emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
        }

        public async Task SendEmail(SendEmailRequest emailRequest)
        {
            try
            {
                var message = await ConfigureEmailRecoveryPasswordAsync(emailRequest);
                _emailSender.SendEmail(message);
            }
            catch (Exception exception)
            {
                _logger.LogError($"An unexpected error occurred - EmailService - SendEmail. {exception}");
                throw;
            }
        }

        public async Task<MailMessage> ConfigureEmailAsync(SendEmailRequest emailRequest)
        {
            var message = new MailMessage
            {
                Subject = emailRequest.Subject,
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true,
                Priority = MailPriority.High,
                Body = $"<html><body><h2>{emailRequest.MessageType}</h2><br /><p>Nome: {emailRequest.Name} </b></p><p>Mensagem: {emailRequest.MessageBody} </p><p>Email: {emailRequest.EmailFrom} </p></body></html>",
                From = new MailAddress(Environment.GetEnvironmentVariable("SMTP_FROM"), Environment.GetEnvironmentVariable("SMTP_NAME"), Encoding.UTF8)
            };

            message.To.Add(new MailAddress(emailRequest.EmailTo));
            message.CC.Add(new MailAddress(emailRequest.EmailCopy));

            return message;
        }

        public async Task<MailMessage> ConfigureEmailRecoveryPasswordAsync(SendEmailRequest emailRequest)
        {
            var url = InsfrastructureConstants.ForgotPasswordUrl;

            try
            {
                var message = new MailMessage
                {
                    Subject = emailRequest.Subject,
                    SubjectEncoding = Encoding.UTF8,
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = true,
                    Priority = MailPriority.High,
                    Body = $"<html><body><h2>{emailRequest.MessageType}</h2><br /></p><p>Mensagem: Você solicitou a recuperação de senha, clique no link {url} para resetar sua senha </p></body></html>",
                    From = new MailAddress(Environment.GetEnvironmentVariable("SMTP_FROM"), Environment.GetEnvironmentVariable("SMTP_NAME"), Encoding.UTF8)
                };

                message.To.Add(new MailAddress(emailRequest.EmailTo));
                message.CC.Add(new MailAddress(emailRequest.EmailCopy));

                return message;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in SMTP email", ex.Message);
                throw;
            }
        }
    }
}
