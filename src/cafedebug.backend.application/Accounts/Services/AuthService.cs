using cafedebug.backend.application.Accounts.DTOs.Response;
using cafedebug.backend.application.Accounts.Interfaces;
using cafedebug.backend.application.Common.Mappings;
using cafedebug_backend.domain.Accounts.Errors;
using cafedebug_backend.domain.Accounts.Services;
using cafedebug_backend.domain.Shared;
using System.Security.Cryptography;
using System.Text;

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

        var userResult = await userService.GetUserAdminByEmail(email);
        
        if (!userResult.IsSuccess)
            return Result.Failure<JWTTokenResponse>(AuthError.InvalidCredentials());

        var checkedPassword = CheckPassword(password, userResult.Value.HashedPassword);

        if (!checkedPassword)
            return Result.Failure<JWTTokenResponse>(AuthError.InvalidCredentials());

        var user = userResult.Value;

        //var tokenResult = await jwtService.GenerateAccesTokenAndRefreshtoken(user);

        //if (!tokenResult.IsSuccess)
        //    return Result.Failure<JWTTokenResponse>(AuthError.TokenGenerationFailed(user.Id));

        //return Result.Success(token);

        throw new NotImplementedException();
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
            return Result.Failure<JWTTokenResponse>(UserError.NotFound());

        var newTokenResult = await jwtService.RefreshTokenAsync(storedRefreshToken.Token);
        
        var newToken = newTokenResult.IsSuccess ? newTokenResult.Value : null;

        if (newToken is null)
            return Result.Failure<JWTTokenResponse>(AuthError.RefreshTokenGenerationFailed());
      
        var response = MappingConfig.ToToken(newToken);
        return Result.Success(response);
    }
    private bool CheckPassword(string password, string passwordHash)
    {
        var passwordHashGenerated = GenerateSHA256(password);

        if (passwordHashGenerated == passwordHash)
            return true;

        return false;
    }

    private string GenerateSHA256(string password)
    {
        using (var sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}