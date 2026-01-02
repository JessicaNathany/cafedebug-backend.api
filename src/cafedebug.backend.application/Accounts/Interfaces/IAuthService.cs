using cafedebug.backend.application.Accounts.DTOs.Response;
using cafedebug_backend.domain.Shared;

namespace cafedebug.backend.application.Accounts.Interfaces;

public interface IAuthService
{
    Task<Result<JWTTokenResponse>> GenerateTokenAsync(string email, string password);
    Task<Result<JWTTokenResponse>> RefreshTokenAsync(string refreshToken);
}