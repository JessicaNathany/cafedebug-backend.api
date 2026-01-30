using cafedebug.backend.application.Accounts.DTOs.Response;
using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Accounts.Tokens;
using cafedebug_backend.domain.Shared;

namespace cafedebug.backend.application.Accounts.Interfaces;
public interface IJWTService
{
    Task<Result<RefreshTokens>> GetByTokenAsync(string token);
    string GenerateResetToken(int userId);
    Task<Result<JWTTokenResponse>> RefreshTokenAsync(string refreshToken);
    Task<Result<JWTTokenResponse>> GenerateToken(string email, string password);
    Task<JWTTokenResponse> GenerateAccesTokenAndRefreshtoken(UserAdmin userAdmin);
}