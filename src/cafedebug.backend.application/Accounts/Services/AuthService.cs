using cafedebug.backend.application.Accounts.DTOs.Response;
using cafedebug.backend.application.Accounts.Interfaces;
using cafedebug.backend.application.Common.Mappings;
using cafedebug_backend.domain.Accounts.Errors;
using cafedebug_backend.domain.Accounts.Services;
using cafedebug_backend.domain.Shared;

namespace cafedebug.backend.application.Accounts.Services;

/// <summary>
/// Service responsible for authentication operations including token generation and refresh
/// </summary>
public class AuthService(IUserService userService, IJWTService jwtService) : IAuthService
{
    public async Task<Result<JWTTokenResponse>> GenerateTokenAsync(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            return Result.Failure<JWTTokenResponse>(AuthError.EmptyCredentials());

        var user = await userService.GetByEmailAsync(email, password);
        if (!user.IsSuccess)
            return Result.Failure<JWTTokenResponse>(AuthError.InvalidCredentials());

        var token = await jwtService.GenerateAccesTokenAndRefreshtoken(user.Value);
        if (token is null)
            return Result.Failure<JWTTokenResponse>(AuthError.TokenGenerationFailed(user.Value.Id));

        var response = MappingConfig.ToToken(token);
        return Result.Success(response);
    }

    public async Task<Result<JWTTokenResponse>> RefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
            return Result.Failure<JWTTokenResponse>(AuthError.RefreshTokenNull());

        var refreshTokenResult = await jwtService.GetByTokenAsync(refreshToken);
        if (!refreshTokenResult.IsSuccess || refreshTokenResult.Value is null)
            return Result.Failure<JWTTokenResponse>(AuthError.RefreshTokenInvalid());

        var storedRefreshToken = refreshTokenResult.Value;

        if (storedRefreshToken.ExpirationDate <= DateTime.UtcNow)
            return Result.Failure<JWTTokenResponse>(AuthError.RefreshTokenExpired());

        var userResult = await userService.GetByIdAsync(storedRefreshToken.UserId);
        if (!userResult.IsSuccess)
            return Result.Failure<JWTTokenResponse>(UserError.NotFound);

        var newToken = await jwtService.RefreshTokenAsync(storedRefreshToken, userResult.Value);
        if (newToken is null)
            return Result.Failure<JWTTokenResponse>(AuthError.RefreshTokenGenerationFailed());
      
        var response = MappingConfig.ToToken(newToken);
        return Result.Success(response);
    }
}