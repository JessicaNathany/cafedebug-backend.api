using cafedebug_backend.domain.Audience;

namespace cafedebug_backend.domain.Messages.Email.Services;

public interface IMailService
{
    Task SendEmailAsync(Contact contact);
}