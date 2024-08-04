using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Jwt;
using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Interfaces.JWT
{
    public interface IJWTService
    {
        Task<JWTToken> GenerateToken(UserAdmin userAdmin);

        Task SaveRefreshTokenAsync(RefreshTokens refreshTokens, CancellationToken cancellationToken);

        Task<Result<RefreshTokens>> GetByTokenAsync(string token);
    }
}
