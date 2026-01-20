using cafedebug_backend.domain.Messages.Email.Request;
using cafedebug_backend.domain.Messages.Email.Services;
using Moq;

namespace cafedebug.backend.api.test.Shared.Setups.Accounts
{
    public class EmailServiceMockSetup(Mock<IEmailService> emailService)
    {
        public void SendEmailSuccess()
        {
            emailService
                .Setup(x => x.SendEmail(It.IsAny<SendEmailRequest>()))
                .Returns(Task.CompletedTask);
        }

        public void SendEmailThrows(Exception exception)
        {
            emailService
                .Setup(x => x.SendEmail(It.IsAny<SendEmailRequest>()))
                .ThrowsAsync(exception);
        }
    }
}
