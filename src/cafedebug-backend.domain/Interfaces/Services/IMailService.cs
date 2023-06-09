using cafedebug_backend.domain.Entities;

namespace cafedebug_backend.domain.Interfaces.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(Contact contact);
    }
}
