using cafedebug_backend.domain.Interfaces.Repositories;
using cafedebug_backend.domain.Jwt;
using cafedebug_backend.domain.Shared.Repositories;

namespace cafedebug_backend.domain.Accounts.Repositories;

public interface IRefreshTokensRepository : IBaseRepository<RefreshTokens>  
{
    Task<RefreshTokens?> GetByTokenAsync(string token);

    Task<RefreshTokens?> GetByTokenByUserIdAsync(int userId);
}