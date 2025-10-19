using cafedebug_backend.domain.Entities;

namespace cafedebug_backend.domain.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<UserAdmin>
{
    Task<UserAdmin?> GetByEmailAsync(string email);
}