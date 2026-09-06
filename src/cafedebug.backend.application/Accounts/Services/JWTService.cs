using cafedebug.backend.application.Accounts.DTOs.Response;
using cafedebug.backend.application.Accounts.Interfaces;
using cafedebug.backend.application.Common.Mappings;
using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Accounts.Errors;
using cafedebug_backend.domain.Accounts.Repositories;
using cafedebug_backend.domain.Accounts.Tokens;
using cafedebug_backend.domain.Interfaces.Repositories;
using cafedebug_backend.domain.Shared;
using cafedebug_backend.infrastructure.Security;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace cafedebug.backend.application.Accounts.Services;

/// <summary>
/// Service responsible for generating and managing JWT tokens.
/// </summary>
public class JWTService(
    JwtSettings jwtSettings,
    IRefreshTokensRepository refreshTokensRepository,
    IUserRepository userRepository,
    TimeProvider timeProvider) : IJWTService
{
    public async Task<Result<JWTTokenResponse>> GenerateToken(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            return Result.Failure<JWTTokenResponse>(TokenError.EmailOrPassordEmpty());

        var user = await userRepository.GetByEmailAsync(email);

        if (user is null)
            return Result.Failure<JWTTokenResponse>(UserError.NotFound(email));

        var hashedPassword = GenerateSHA256(password);
        if (user.HashedPassword != hashedPassword)
            return Result.Failure<JWTTokenResponse>(TokenError.PasswordInvalid());

        var token = await GenerateAccesTokenAndRefreshtoken(user);

        if (token is null)
            return Result.Failure<JWTTokenResponse>(TokenError.ErrorCreatingToken(user.Email));

        return Result.Success(token);
    }

    private string GenerateSHA256(string password)
    {
        using (var sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
                builder.Append(bytes[i].ToString("x2"));

            return builder.ToString();
        }
    }

    public async Task<Result<JWTTokenResponse>> RefreshTokenAsync(string refreshToken)
    {
        var refreshTokenResult = await GetByTokenAsync(refreshToken);

        var now = timeProvider.GetUtcNow().UtcDateTime;
        if (!refreshTokenResult.IsSuccess || refreshTokenResult.Value == null || refreshTokenResult.Value.ExpirationDate <= now)
            return Result.Failure<JWTTokenResponse>(AuthError.RefreshTokenInvalid());

        var refreshTokenEntity = refreshTokenResult.Value;

        // generate new token
        string generatedToken;
        var randomNumber = new byte[32];
        using (var randonNumberGenerator = RandomNumberGenerator.Create())
        {
            randonNumberGenerator.GetBytes(randomNumber);
            generatedToken = Convert.ToBase64String(randomNumber);
        }

        var token = generatedToken.Replace("+", string.Empty).Replace("=", string.Empty).Replace("/", string.Empty);
        var expirationDate = now.AddMinutes(jwtSettings.RefreshTokenValidForMinutes);

        refreshTokenEntity.UpdateToken(token, expirationDate, now);
        await refreshTokensRepository.UpdateAsync(refreshTokenEntity);

        // get user by token user id
        var user = await userRepository.GetByIdAsync(refreshTokenEntity.UserId);
        if (user is null)
            return Result.Failure<JWTTokenResponse>(UserError.NotFound(""));

        var identity = GetClaimsIdentity(user);
        var jsonSecurityHandler = new JwtSecurityTokenHandler();
        var accessToken = CreateAccessToken(jsonSecurityHandler, identity, now);

        var response = MappingConfig.ToToken(JWTToken.Create(
            accessToken, refreshTokenEntity,
            TokenType.Bearer.ToString(),
            (long)TimeSpan.FromMinutes(jwtSettings.ValidForMinutes).TotalSeconds));

        return Result.Success(response);
    }

    public string GenerateResetToken(int userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = jwtSettings.SigningCredentials.Key as SymmetricSecurityKey;
        var now = timeProvider.GetUtcNow().UtcDateTime;

        if (key is null)
            throw new InvalidOperationException("Signing key is not a symmetric key.");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("userId", userId.ToString()) }),
            IssuedAt = now,
            NotBefore = now,
            Expires = now.AddMinutes(15),
            SigningCredentials = jwtSettings.SigningCredentials
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<JWTTokenResponse> GenerateAccesTokenAndRefreshtoken(UserAdmin userAdmin)
    {
        var identity = GetClaimsIdentity(userAdmin);

        var jsonSecurityHandler = new JwtSecurityTokenHandler();

        var now = timeProvider.GetUtcNow().UtcDateTime;
        var accessToken = CreateAccessToken(jsonSecurityHandler, identity, now);
        var createRefreshToken = await CreateRefreshToken(userAdmin.Id, userAdmin.Name, now);

        return new JWTTokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = new RefreshTokenResponse
            {
                Token = createRefreshToken.Token,
                ExpirationDate = createRefreshToken.ExpirationDate
            },
            TokenType = TokenType.Bearer.ToString(),
            ExpiresIn = (long)TimeSpan.FromMinutes(jwtSettings.ValidForMinutes).TotalSeconds
        };
    }

    private ClaimsIdentity GetClaimsIdentity(UserAdmin userAdmin)
    {
        var identity = new ClaimsIdentity
        (
            new GenericIdentity(userAdmin.Email),
            new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, userAdmin.Name)
            }
        );

        return identity;
    }

    public async Task<Result<RefreshTokens>> GetByTokenAsync(string token)
    {
        var getToken = await refreshTokensRepository.GetByTokenAsync(token);

        if (getToken is null)
            return Result.Failure<RefreshTokens>(AuthError.RefreshTokenInvalid());

        return Result.Success(getToken);
    }

    private string CreateAccessToken(JwtSecurityTokenHandler tokenHandler, ClaimsIdentity identity, DateTime now)
    {
        var securityToken = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = identity,
            Issuer = jwtSettings.Issuer,
            Audience = jwtSettings.Audience,
            IssuedAt = now,
            NotBefore = now,
            Expires = now.AddMinutes(jwtSettings.ValidForMinutes),
            SigningCredentials = jwtSettings.SigningCredentials
        });

        return tokenHandler.WriteToken(securityToken);
    }

    private async Task<RefreshTokens> CreateRefreshToken(int userId, string userName, DateTime now)
    {
        string generatedToken;
        var randomNumber = new byte[32];

        using (var randonNumberGenerator = RandomNumberGenerator.Create())
        {
            randonNumberGenerator.GetBytes(randomNumber);
            generatedToken = Convert.ToBase64String(randomNumber);
        }

        var token = generatedToken.Replace("+", string.Empty).Replace("=", string.Empty).Replace("/", string.Empty);
        var expirationDate = now.AddMinutes(jwtSettings.RefreshTokenValidForMinutes);

        var refreshTokenByUser = await refreshTokensRepository.GetByTokenByUserIdAsync(userId);

        if (refreshTokenByUser is null)
        {
            var refreshToken = RefreshTokens.Create(userId, userName, token, expirationDate, now, now);
            await refreshTokensRepository.SaveAsync(refreshToken);
            return refreshToken;
        }

        refreshTokenByUser.UpdateToken(token, expirationDate, now);
        await refreshTokensRepository.UpdateAsync(refreshTokenByUser);
        return refreshTokenByUser;
    }
}
