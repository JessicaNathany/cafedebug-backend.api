using cafedebug_backend.domain.Accounts.Tokens;
using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Accounts.Services;

public interface IJWTService
{
    Task<JWTToken> GenerateAccesTokenAndRefreshtoken(UserAdmin userAdmin);

    Task<Result<RefreshTokens>> GetByTokenAsync(string token);

    string GenerateResetToken(int userId);

    Task<JWTToken> RefreshTokenAsync(RefreshTokens refreshToken, UserAdmin userAdmin);
}