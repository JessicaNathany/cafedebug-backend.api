using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Jwt;
using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Interfaces.JWT
{
    public interface IJWTService
    {
        Task<JWTToken> GenerateAccesTokenAndRefreshtoken(UserAdmin userAdmin);

        Task<Result<RefreshTokens>> GetByTokenAsync(string token);

        Task UpdateRefreshToken(RefreshTokens oldRefreshTokens, RefreshTokens newRefreshTokens);

        Task<JWTToken> GenerateNewAccessToken(UserAdmin userAdmin, RefreshTokens refreshTokens);

        string GenerateResetToken(int userId);
    }
}
