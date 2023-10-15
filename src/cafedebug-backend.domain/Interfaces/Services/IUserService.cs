using cafedebug_backend.domain.Entities;

namespace cafedebug_backend.domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserAdmin> GetByLoginAndPasswordAsync(string email, string password, CancellationToken cancellationToken);
    }
}
