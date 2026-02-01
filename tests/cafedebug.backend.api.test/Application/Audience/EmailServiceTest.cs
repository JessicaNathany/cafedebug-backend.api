using System.Net.Mail;
using cafedebug.backend.application.Audience.Interfaces;
using cafedebug_backend.domain.Messages.Email.Request;
using cafedebug_backend.infrastructure.Constants;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace cafedebug.backend.api.test.Application.Audience
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
            var loggerMock = new Mock<ILogger<cafedebug.backend.application.Audience.Services.EmailService>>();
            var emailSenderMock = new Mock<IEmailSenderService>();
            var emailService = new cafedebug.backend.application.Audience.Services.EmailService(loggerMock.Object, emailSenderMock.Object);

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

        [Fact]
        public async Task ConfigureEmailAsync_ValidRequest_ReturnsCorrectMailMessage()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<cafedebug.backend.application.Audience.Services.EmailService>>();
            var emailSenderMock = new Mock<IEmailSenderService>();
            var emailService = new cafedebug.backend.application.Audience.Services.EmailService(loggerMock.Object, emailSenderMock.Object);

            var emailRequest = new SendEmailRequest
            {
                EmailTo = "jn.devtemp@gmail.com",
                Subject = "Café debug unit test",
                MessageType = "Recovery",
                EmailCopy = "debugcafe@gmail.com",
                Name = "John Doe",
                MessageBody = "This is a test message.",
                EmailFrom = "john.doe@example.com"
            };

            // Act
            var mailMessage = await emailService.ConfigureEmailAsync(emailRequest);

            // Assert
            Assert.NotNull(mailMessage);
            Assert.Equal(emailRequest.Subject, mailMessage.Subject);
            Assert.Equal(emailRequest.EmailTo, mailMessage.To[0].Address);
            Assert.Equal(emailRequest.EmailCopy, mailMessage.CC[0].Address);
            Assert.Contains(emailRequest.Name, mailMessage.Body);
            Assert.Contains(emailRequest.MessageBody, mailMessage.Body);
            Assert.Contains(emailRequest.EmailFrom, mailMessage.Body);
        }

        [Fact]
        public async Task ConfigureEmailRecoveryPasswordAsync_ValidRequest_ReturnsCorrectMailMessage()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<cafedebug.backend.application.Audience.Services.EmailService>>();
            var emailSenderMock = new Mock<IEmailSenderService>();
            var emailService = new cafedebug.backend.application.Audience.Services.EmailService(loggerMock.Object, emailSenderMock.Object);

            var emailRequest = new SendEmailRequest
            {
                EmailTo = "jn.devtemp@gmail.com",
                Subject = "Café debug unit test - Password Recovery",
                MessageType = "Password Recovery",
                EmailCopy = "debugcafe@gmail.com"
            };

            // Act
            var mailMessage = await emailService.ConfigureEmailRecoveryPasswordAsync(emailRequest);

            // Assert
            Assert.NotNull(mailMessage);
            Assert.Equal(emailRequest.Subject, mailMessage.Subject);
            Assert.Equal(emailRequest.EmailTo, mailMessage.To[0].Address);
            Assert.Equal(emailRequest.EmailCopy, mailMessage.CC[0].Address);
            Assert.Contains("recuperação de senha", mailMessage.Body);
            Assert.Contains(InsfrastructureConstants.ForgotPasswordUrl, mailMessage.Body);
        }
    }
}