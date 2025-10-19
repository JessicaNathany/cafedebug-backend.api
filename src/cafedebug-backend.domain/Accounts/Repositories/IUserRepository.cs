using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Shared.Repositories;

namespace cafedebug_backend.domain.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<UserAdmin>
{
    Task<UserAdmin?> GetByEmailAsync(string email);
}