using cafedebug.backend.application.Accounts.DTOs.Response;
using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Shared;

namespace cafedebug.backend.application.Accounts.Interfaces
{
    public interface IUserService
    {
        Task<Result<UserAdminResponse>> GetByLoginAndPasswordAsync(string email, string password);

        Task<Result<UserAdminResponse>> CreateAsync(string email, string password);

        Task<Result<UserAdminResponse>> UpdateAsync(UserAdmin userAdmin);

        Task<Result<UserAdminResponse>> GetByIdAsync(int id);

        Task<Result> DeleteAsync(int id);

        Task<Result<UserAdminResponse>> GetUserAdminByEmail(string email);
    }
}
