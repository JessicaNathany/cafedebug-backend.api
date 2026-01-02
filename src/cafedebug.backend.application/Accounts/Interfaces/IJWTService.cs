using cafedebug.backend.application.Accounts.DTOs.Response;
using cafedebug_backend.domain.Accounts.Tokens;
using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Accounts.Services;

public interface IJWTService
{
    Task<Result<RefreshTokens>> GetByTokenAsync(string token);

    string GenerateResetToken(int userId);

    Task<Result<JWTToken>> RefreshTokenAsync(string refreshToken);

    Task<Result<JWTTokenResponse>> GenerateToken(string email, string password);
}