using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<Result<UserAdmin>> GetByLoginAndPasswordAsync(string email, string password);

        Task<Result<UserAdmin>> CreateAsync(string email, string password);

        Task<Result<UserAdmin>> UpdateAsync(UserAdmin userAdmin);

        Task<Result<UserAdmin>> GetByIdAsync(int id);

        Task<Result> DeleteAsync(int id);

        Task<Result<UserAdmin>> GetByEmailAsync(string email, string password);

        Task<Result<UserAdmin>> GetUserAdminByEmail(string email);
    }
}
