using cafedebug.backend.application.Constants;
using cafedebug_backend.domain.Interfaces.Services;
using cafedebug_backend.domain.Request;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace cafedebug.backend.application.Service
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
           _logger = logger;
        }

        public async Task SendEmail(SendEmailRequest emailRequest)
        {
            var message = ConfigureEmailAsync(emailRequest);

            var client = new SmtpClient
            {
                Port = Convert.ToInt16(Environment.GetEnvironmentVariable(InsfrastructureConstants.SmtpPort)),
                Host = Environment.GetEnvironmentVariable(InsfrastructureConstants.SmtpServer),
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Environment.GetEnvironmentVariable("SMTP_FROM"), Environment.GetEnvironmentVariable("SMTP_PASSWORD")),
            };

            client.Send(message);
        }

        private MailMessage ConfigureEmailAsync(SendEmailRequest emailRequest)
        {
            var message = new MailMessage
            {
                Subject = emailRequest.Subject,
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true,
                Priority = MailPriority.High,
                Body = $"<html><body><h2>{emailRequest.MessageBody}</h2><br /><p>Nome: {emailRequest.Name} </b></p><p>Mensagem: {emailRequest.Message} </p><p>Email: {emailRequest.Email} </p></body></html>",
                From = new MailAddress(Environment.GetEnvironmentVariable("SMTP_PASSWORD"), Environment.GetEnvironmentVariable("SMTP_NAME"), Encoding.UTF8)
            };

            message.To.Add(new MailAddress(emailRequest.EmailTo));
            message.CC.Add(new MailAddress(emailRequest.EmailCopy));

            return message;
        }
    }
}
