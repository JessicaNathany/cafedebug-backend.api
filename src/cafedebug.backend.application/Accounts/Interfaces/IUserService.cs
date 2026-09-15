using cafedebug.backend.application.Accounts.DTOs.Requests;
using cafedebug.backend.application.Accounts.DTOs.Response;
using cafedebug.backend.application.Common.Pagination;
using cafedebug_backend.domain.Shared;

namespace cafedebug.backend.application.Accounts.Interfaces;
public interface IUserService
{
    Task<Result<UserAdminResponse>> GetByLoginAndPasswordAsync(string email, string password);

    Task<Result<UserAdminResponse>> CreateAsync(UserAdminRequest request);

    Task<Result<UserAdminResponse>> UpdateAsync(UserAdminRequest request, int id);

    Task<Result<PagedResult<UserAdminResponse>>> GetAllAsync(PageRequest request);

    Task<Result<UserAdminResponse>> GetByIdAsync(int id);

    Task<Result> DeleteAsync(int id);

    Task<Result<UserAdminResponse>> GetUserAdminByEmail(string email);
}
