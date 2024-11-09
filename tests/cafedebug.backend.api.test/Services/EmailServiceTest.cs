using cafedebug.backend.application.Service;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace cafedebug.backend.api.test.Services
{
    public class EmailServiceTest
    {
        private readonly AutoMocker _autoMocker;
        public EmailServiceTest()
        {
            _autoMocker = new AutoMocker();
            var loggerMock = Mock.Of<ILogger<EmailService>>();
        }

        [Fact]
        public async Task SendEmail_Should_Be_Success()
        {
            //Arrange

            // Act
            var emailService = _autoMocker.CreateInstance<EmailService>();
        }
    }
}
