using cafedebug.backend.application.Service;
using cafedebug_backend.domain.Interfaces.Services;
using cafedebug_backend.domain.Request;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.AutoMock;
using System.Net;
using System.Net.Mail;
using Xunit;

namespace cafedebug.backend.api.test.Services
{
    public class EmailServiceTest
    {
        private readonly AutoMocker _autoMocker;
        public EmailServiceTest()
        {
            _autoMocker = new AutoMocker();
        }

        [Fact]
        public void CheckEnvironmentVariables()
        {
            var smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
            var smtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER");

            Console.WriteLine($"SMTP Password: {smtpPassword}");
            Console.WriteLine($"SMTP Server: {smtpServer}");

            if (string.IsNullOrEmpty(smtpPassword) || string.IsNullOrEmpty(smtpServer))
            {
                throw new InvalidOperationException("SMTP settings are not configured properly.");
            }
        }


        [Fact]
        public async Task SendEmail_ValidRequest_CallsEmailSender()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<EmailService>>();
            var emailSenderMock = new Mock<IEmailSender>();
            var emailService = new EmailService(loggerMock.Object, emailSenderMock.Object);

            var emailRequest = new SendEmailRequest
            {
                EmailTo = "jn.devtemp@gmail.com",
                Subject = "Café debug unit test",
                MessageType = "Recovery",
                EmailCopy = "debugcafe@gmail.com",
            };

            emailSenderMock.Setup(x => x.SendEmail(It.IsAny<MailMessage>())).Verifiable("The email was not sent.");

            // Act
            await emailService.SendEmail(emailRequest);

            // Assert
            emailSenderMock.Verify(); 
        }
    }
}

