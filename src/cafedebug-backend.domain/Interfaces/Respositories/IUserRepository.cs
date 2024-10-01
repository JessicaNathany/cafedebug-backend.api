using cafedebug_backend.domain.Entities;

namespace cafedebug_backend.domain.Interfaces.Respositories
{
    public interface IUserRepository : IBaseRepository<UserAdmin>
    {
        Task<UserAdmin> GetByEmailAsync(string email);
    }
}
