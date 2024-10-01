using cafedebug_backend.domain.Request;

namespace cafedebug_backend.domain.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmail(SendEmailRequest emailRequest);
    }
}
