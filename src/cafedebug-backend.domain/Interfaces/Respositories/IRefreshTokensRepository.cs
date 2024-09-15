using cafedebug_backend.domain.Jwt;

namespace cafedebug_backend.domain.Interfaces.Respositories
{
    public interface IRefreshTokensRepository : IBaseRepository<RefreshTokens>  
    {
        Task<RefreshTokens> GetByTokenAsync(string token);

        Task<RefreshTokens> GetByTokenByUserIdAsync(int userId);
    }
}
