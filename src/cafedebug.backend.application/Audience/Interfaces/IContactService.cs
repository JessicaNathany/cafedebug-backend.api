using cafedebug_backend.domain.Audience;

namespace cafedebug.backend.application.Audience.Interfaces;

public interface IContactService 
{
    Task Save(Contact contact);
}