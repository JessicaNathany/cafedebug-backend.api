using cafedebug.backend.application.Accounts.DTOs.Response;
using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Accounts.Errors;
using cafedebug_backend.domain.Accounts.Repositories;
using cafedebug_backend.domain.Accounts.Services;
using cafedebug_backend.domain.Accounts.Tokens;
using cafedebug_backend.domain.Interfaces.Repositories;
using cafedebug_backend.domain.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;

namespace cafedebug_backend.infrastructure.Security;

/// <summary>
/// Service responsible for generating and managing JWT tokens.
/// </summary>
public class JWTService(
    JwtSettings jwtSettings, 
    IRefreshTokensRepository refreshTokensRepository, 
    IUserRepository userRepository,
    IPasswordHasher<UserAdmin> passwordHasher) : IJWTService
{
    public async Task<Result<JWTTokenResponse>> GenerateToken(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            return Result.Failure<JWTTokenResponse>(TokenError.EmailOrPassordEmpty());

        var user = await userRepository.GetByEmailAsync(email);

        if (user is null)
            return Result.Failure<JWTTokenResponse>(UserError.NotFound(email));

        var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.HashedPassword, password);

        if (passwordVerificationResult is PasswordVerificationResult.Failed)
                return Result.Failure<JWTTokenResponse>(TokenError.PasswordInvalid());

        var token = await GenerateAccesTokenAndRefreshtoken(user);

        if(token is null)
            return Result.Failure<JWTTokenResponse>(TokenError.ErrorCreatingToken(user.Email));

        return Result.Success(token);
    }

    public async Task<Result<JWTToken>> RefreshTokenAsync(string refreshToken)
    {

        //try
        //{


        //    var refreshTokenResult = await _jWTService.GetByTokenAsync(refreshTokenRequest.RefreshToken);

        //    if (refreshTokenResult.Value == null || refreshTokenResult.Value.ExpirationDate <= DateTime.UtcNow)
        //    {
        //        _logger.LogWarning("Invalid or expired refresh token.");
        //        return Unauthorized("Invalid or expired refresh token.");
        //    }

        //    var userResult = await _userService.GetByIdAsync(refreshTokenResult.Value.UserId);

        //    if (!userResult.IsSuccess)
        //    {
        //        _logger.LogWarning("User not found for refresh token.");
        //        return NotFound("User not found.");
        //    }

        //    var token = await _jWTService.RefreshTokenAsync(refreshTokenResult.Value, userResult.Value);

        //    if (token is null)
        //    {
        //        _logger.LogError("Error creating new token during refresh.");
        //        return BadRequest("Error creating token.");
        //    }

        //    return Ok(token);
        //}
        //catch (UnauthorizedAccessException)
        //{
        //    _logger.LogWarning("Unauthorized access during refresh token.");
        //    return Unauthorized();
        //}
        //catch (NullReferenceException ex)
        //{
        //    _logger.LogError(ex, "NullReferenceException in refresh token endpoint.");
        //    return StatusCode(500, "Internal server error");
        //}
        //catch (Exception ex)
        //{
        //    _logger.LogError(ex, "Unexpected error in refresh token endpoint.");
        //    return StatusCode(500, "Internal server error");
        //}


        var refreshTokenResult = await GetByTokenAsync(refreshToken);

        if (!refreshTokenResult.IsSuccess || refreshTokenResult.Value == null || refreshTokenResult.Value.ExpirationDate <= DateTime.UtcNow)
            return Result.Failure<JWTToken>(AuthError.RefreshTokenInvalid());

        var refreshTokenEntity = refreshTokenResult.Value;

        // Gerar novo refresh token
        string generatedToken;
        var randomNumber = new byte[32];
        using (var randonNumberGenerator = RandomNumberGenerator.Create())
        {
            randonNumberGenerator.GetBytes(randomNumber);
            generatedToken = Convert.ToBase64String(randomNumber);
        }

        var token = generatedToken.Replace("+", string.Empty).Replace("=", string.Empty).Replace("/", string.Empty);
        var expirationDate = DateTime.UtcNow.AddMinutes(jwtSettings.RefreshTokenValidForMinutes);

        refreshTokenEntity.UpdateToken(token, expirationDate);
        await refreshTokensRepository.UpdateAsync(refreshTokenEntity);

        // Buscar o usuário para gerar novo access token
        var user = await userRepository.GetByIdAsync(refreshTokenEntity.UserId);
        if (user is null)
            return Result.Failure<JWTToken>(UserError.NotFound(""));

        // Gerar novo access token
        var identity = GetClaimsIdentity(user);
        var jsonSecurityHandler = new JwtSecurityTokenHandler();
        var securityToken = jsonSecurityHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = identity,
            Issuer = jwtSettings.Issuer,
            Audience = jwtSettings.Audience,
            IssuedAt = jwtSettings.IssuedAt,
            NotBefore = jwtSettings.NotBefore,
            Expires = jwtSettings.AccessTokenExpiration,
            SigningCredentials = jwtSettings.SigningCredentials
        });
        var accessToken = jsonSecurityHandler.WriteToken(securityToken);

        return Result.Success(JWTToken.Create(
            accessToken, refreshTokenEntity,
            TokenType.Bearer.ToString(),
            (long)TimeSpan.FromMinutes(jwtSettings.ValidForMinutes).TotalSeconds));
    }

    public string GenerateResetToken(int userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = jwtSettings.SigningCredentials.Key as SymmetricSecurityKey;

        if (key == null)
            throw new InvalidOperationException("Signing key is not a symmetric key.");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("userId", userId.ToString()) }),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = jwtSettings.SigningCredentials
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private async Task<JWTTokenResponse> GenerateAccesTokenAndRefreshtoken(UserAdmin userAdmin)
    {
        var identity = GetClaimsIdentity(userAdmin);

        var jsonSecurityHandler = new JwtSecurityTokenHandler();

        var securityToken = jsonSecurityHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = identity,
            Issuer = jwtSettings.Issuer,
            Audience = jwtSettings.Audience,
        });

        var accessToken = jsonSecurityHandler.WriteToken(securityToken);
        var createRefreshToken = await CreateRefreshToken(userAdmin.Id, userAdmin.Name);

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

    private async Task<RefreshTokens> CreateRefreshToken(int userId, string userName)
    {
        string generatedToken;
        var randomNumber = new byte[32];

        using (var randonNumberGenerator = RandomNumberGenerator.Create())
        {
            randonNumberGenerator.GetBytes(randomNumber);
            generatedToken = Convert.ToBase64String(randomNumber);
        }

        var token = generatedToken.Replace("+", string.Empty).Replace("=", string.Empty).Replace("/", string.Empty);
        var expirationDate = DateTime.UtcNow.AddMinutes(jwtSettings.RefreshTokenValidForMinutes);
        var now = DateTime.UtcNow;

        var refreshTokenByUser = await refreshTokensRepository.GetByTokenByUserIdAsync(userId);

        if (refreshTokenByUser != null)
        {
            refreshTokenByUser.UpdateToken(token, expirationDate);
            await refreshTokensRepository.UpdateAsync(refreshTokenByUser);
            return refreshTokenByUser;
        }
        else
        {
            var refreshToken = RefreshTokens.Create(userId, userName, token, expirationDate, now);
            await refreshTokensRepository.SaveAsync(refreshToken);
            return refreshToken;
        }
    }
}