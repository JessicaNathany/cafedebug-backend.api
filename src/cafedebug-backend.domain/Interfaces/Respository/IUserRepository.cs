using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respositories;

namespace cafedebug_backend.domain.Interfaces.Respository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetByLoginAndPassword(string login, string senha);
    }
}
