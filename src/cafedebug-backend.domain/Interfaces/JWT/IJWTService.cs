using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Jwt;
using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Interfaces.JWT
{
    public interface IJWTService
    {
        Task<JWTToken> GenerateAccesTokenAndRefreshtoken(UserAdmin userAdmin);

        Task<Result<RefreshTokens>> GetByTokenAsync(string token);

        string GenerateResetToken(int userId);

        Task<JWTToken> RefreshTokenAsync(RefreshTokens refreshToken, UserAdmin userAdmin);
    }
}
