using cafedebug.backend.application.Service;
using cafedebug_backend.domain.Interfaces.Services;
using cafedebug_backend.domain.Request;
using cafedebug_backend.domain.Shared;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace cafedebug.backend.api.test.Services
{
    public class AccountServiceTest
    {
        private readonly AutoMocker _autoMocker;
        private readonly Mock<IEmailService> _emailServiceMock;

        public AccountServiceTest()
        {
            _autoMocker = new AutoMocker();
            _emailServiceMock = new Mock<IEmailService>();
        }

        [Fact]
        public async Task SendEmailForgotPassword_InvalidEmail_Return_Error()
        {
            // Arrange
            var request = new SendEmailRequest
            {
                Email = "test.test.com",
                EmailCopy = "jessica.nathany@local.com",
                EmailTo = "debugcafe@local.com",
                Name = "Test",
                MessageType = "Reset Password"
            };

            // Act
            var accountService = _autoMocker.CreateInstance<AccountService>();

            _autoMocker.GetMock<IEmailService>()
                .Setup(x => x.SendEmail(request))
                .Returns(Task.FromResult<Result>);

            var result = await accountService.SendEmailForgotPassword(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            Assert.Equal("Email invalid.", result.Error);
        }

        [Fact]
        public async Task SendEmailForgotPassword_Should_be_Success()
        {
            // Arrange
            var request = new SendEmailRequest
            {
                Email = "test@test.com",
                EmailCopy = "jessica.nathany@local.com",
                EmailTo = "debugcafe@local.com",
                Name = "Test",
                MessageType = "Reset Password"
            };

            // Act
            var accountService = _autoMocker.CreateInstance<AccountService>();

            _autoMocker.GetMock<IEmailService>()
                .Setup(x => x.SendEmail(request));

            var result = await accountService.SendEmailForgotPassword(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("", result.Error);
        }
    }
}
