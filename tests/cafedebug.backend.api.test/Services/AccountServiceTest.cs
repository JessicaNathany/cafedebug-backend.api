using cafedebug_backend.domain.Accounts;
using cafedebug.backend.application.Service;
using cafedebug_backend.domain.Errors;
using cafedebug_backend.domain.Messages.Email.Request;
using cafedebug_backend.domain.Messages.Email.Services;
using cafedebug_backend.domain.Shared;
using cafedebug.backend.application.Accounts;
using cafedebug.backend.application.Accounts.Services;
using Moq.AutoMock;
using Xunit;

namespace cafedebug.backend.api.test.Services
{
    public class AccountServiceTest
    {
        private readonly AutoMocker _autoMocker;

        public AccountServiceTest()
        {
            _autoMocker = new AutoMocker();
        }

        [Fact]
        public async Task SendEmailForgotPassword_Should_be_Success()
        {
            // Arrange
            var request = new SendEmailRequest
            {
                EmailFrom = "debugcafe@local.com",
                EmailCopy = "jessica.nathany@local.com",
                EmailTo = "jn.devtemp@gmail.com",
                Name = "Test",
                MessageType = "Reset Password"
            };

            // Act
            var accountService = _autoMocker.CreateInstance<AccountService>();

            _autoMocker.GetMock<IEmailService>().Setup(x => x.SendEmail(request));

            var result = await accountService.SendEmailForgotPassword(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(Error.None, result.Error);
        }

        [Fact]
        public async Task ChangePassword_Should_be_success()
        {
            // Arrange
            var user = new UserAdmin
            {

            };

            // Act


            // Assert
        }
    }
}
