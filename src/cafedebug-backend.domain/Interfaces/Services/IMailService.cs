using cafedebug_backend.domain.Audience;

namespace cafedebug_backend.domain.Interfaces.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(Contact contact);
    }
}
