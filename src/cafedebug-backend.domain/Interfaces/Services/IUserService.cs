using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<Result<UserAdmin>> GetByLoginAndPasswordAsync(string email, string password, CancellationToken cancellationToken);

        Task<Result<UserAdmin>> CreateAsync(string email, string password, CancellationToken cancellationToken);

        Task<Result<UserAdmin>> UpdateAsync(UserAdmin userAdmin, CancellationToken cancellationToken);

        Task<Result<UserAdmin>> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);

        Task<Result<UserAdmin>> GettByEmailAsync(string email, CancellationToken cancellationToken);
    }
}
