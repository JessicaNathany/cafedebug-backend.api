using cafedebug_backend.domain.Entities;

namespace cafedebug_backend.domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<User> GetByLoginAndPasswordAsync(string login, string Password);
    }
}
