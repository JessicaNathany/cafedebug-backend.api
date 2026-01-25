using cafedebug_backend.domain.Messages.Email.Request;
using cafedebug_backend.domain.Messages.Email.Services;
using Moq;

namespace cafedebug.backend.api.test.Shared.Verifications.Account
{
    public class EmailServiceVerification(Mock<IEmailService> emailService)
    {
        public void VerifyEmailSent(Times times)
        {
            emailService.Verify(x => x.SendEmail(It.IsAny<SendEmailRequest>()), times);
        }

        public void VerifyEmailSent(SendEmailRequest request, Times times)
        {
            emailService.Verify(x => x.SendEmail(request), times);
        }
    }
}