using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.JWT;
using cafedebug_backend.domain.Jwt;
using cafedebug_backend.domain.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using cafedebug_backend.domain.Accounts.Errors;
using cafedebug_backend.domain.Interfaces.Repositories;

namespace cafedebug.backend.application.Service;

public class JWTService : IJWTService
{
    private readonly JwtSettings _jtwSettings;
    private readonly ILogger<JWTService> _logger;
    private readonly IRefreshTokensRepository _refreshTokensRepository;

    public JWTService(JwtSettings jwttSettings, ILogger<JWTService> logger,
        IRefreshTokensRepository refreshTokensRepository)
    {
        _jtwSettings = jwttSettings;
        _logger = logger;
        _refreshTokensRepository = refreshTokensRepository;
    }

    public async Task<JWTToken> GenerateAccesTokenAndRefreshtoken(UserAdmin userAdmin)
    {
        var identity = GetClaimsIdentity(userAdmin);

        var jsonSecurityHandler = new JwtSecurityTokenHandler();

        var securityToken = jsonSecurityHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = identity,
            Issuer = _jtwSettings.Issuer,
            Audience = _jtwSettings.Audience,
            IssuedAt = _jtwSettings.IssuedAt,
            NotBefore = _jtwSettings.NotBefore,
            Expires = _jtwSettings.AccessTokenExpiration,
            SigningCredentials = _jtwSettings.SigningCredentials
        });

        var accessToken = jsonSecurityHandler.WriteToken(securityToken);
        var createRefreshToken = await CreateRefreshToken(userAdmin.Id, userAdmin.Name);

        return JWTToken.Create(
            accessToken, createRefreshToken,
            TokenType.Bearer.ToString(),
            (long)TimeSpan.FromMinutes(_jtwSettings.ValidForMinutes).TotalSeconds);
    }

    public async Task<Result<RefreshTokens>> GetByTokenAsync(string token)
    {
        var getToken = await _refreshTokensRepository.GetByTokenAsync(token);

        if (getToken is null)
        {
            _logger.LogWarning($"Token not found.");
            return Result.Failure<RefreshTokens>(TokenError.NotFound);
        }

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
        var expirationDate = DateTime.UtcNow.AddMinutes(_jtwSettings.RefreshTokenValidForMinutes);
        var now = DateTime.UtcNow;

        var refreshTokenByUser = await _refreshTokensRepository.GetByTokenByUserIdAsync(userId);

        if (refreshTokenByUser != null)
        {
            refreshTokenByUser.UpdateToken(token, expirationDate);
            await _refreshTokensRepository.UpdateAsync(refreshTokenByUser);
            return refreshTokenByUser;
        }
        else
        {
            var refreshToken = RefreshTokens.Create(userId, userName, token, expirationDate, now);
            await _refreshTokensRepository.SaveAsync(refreshToken);
            return refreshToken;
        }
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

    public string GenerateResetToken(int userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = _jtwSettings.SigningCredentials.Key as SymmetricSecurityKey;
        if (key == null)
            throw new InvalidOperationException("Signing key is not a symmetric key.");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("userId", userId.ToString()) }),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = _jtwSettings.SigningCredentials
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<JWTToken> RefreshTokenAsync(RefreshTokens refreshToken, UserAdmin userAdmin)
    {
        string generatedToken;
        var randomNumber = new byte[32];
        using (var randonNumberGenerator = RandomNumberGenerator.Create())
        {
            randonNumberGenerator.GetBytes(randomNumber);
            generatedToken = Convert.ToBase64String(randomNumber);
        }

        var token = generatedToken.Replace("+", string.Empty).Replace("=", string.Empty).Replace("/", string.Empty);

        var expirationDate = DateTime.UtcNow.AddMinutes(_jtwSettings.RefreshTokenValidForMinutes);

        refreshToken.UpdateToken(token, expirationDate);
        await _refreshTokensRepository.UpdateAsync(refreshToken);

        // Gere novo access token
        var identity = GetClaimsIdentity(userAdmin);
        var jsonSecurityHandler = new JwtSecurityTokenHandler();
        var securityToken = jsonSecurityHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = identity,
            Issuer = _jtwSettings.Issuer,
            Audience = _jtwSettings.Audience,
            IssuedAt = _jtwSettings.IssuedAt,
            NotBefore = _jtwSettings.NotBefore,
            Expires = _jtwSettings.AccessTokenExpiration,
            SigningCredentials = _jtwSettings.SigningCredentials
        });
        var accessToken = jsonSecurityHandler.WriteToken(securityToken);

        return JWTToken.Create(
            accessToken, refreshToken,
            TokenType.Bearer.ToString(),
            (long)TimeSpan.FromMinutes(_jtwSettings.ValidForMinutes).TotalSeconds);
    }
}