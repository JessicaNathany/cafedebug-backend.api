using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.JWT;
using cafedebug_backend.domain.Interfaces.Respositories;
using cafedebug_backend.domain.Jwt;
using cafedebug_backend.domain.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;

namespace cafedebug.backend.application.Service
{
    public class JWTService : IJWTService
    {
        private readonly JwtSettings _jtwSettings;
        private readonly ILogger<JWTService> _logger;
        private readonly IRefreshTokensRepository _refreshTokensRepository;

        public JWTService(JwtSettings jwttSettings, ILogger<JWTService> logger, IRefreshTokensRepository refreshTokensRepository)
        {
            _jtwSettings = jwttSettings;
            _logger = logger;   
            _refreshTokensRepository = refreshTokensRepository;
        }

        public async Task<JWTToken> GenerateToken(UserAdmin userAdmin)
        {
            try
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
                var createRefreshToken = CreateRefreshToken(userAdmin.Name);

                return JWTToken.Create(
                    accessToken, createRefreshToken,
                    TokenType.Bearer.ToString(),
                    (long)TimeSpan.FromMinutes(_jtwSettings.ValidForMinutes).TotalSeconds);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Result<RefreshTokens>>  GetByTokenAsync(string token)
        {
            try
            {
                var getToken = await _refreshTokensRepository.GetByTokenAsync(token);

                if(getToken is null)
                {
                    _logger.LogWarning($"Token not found.");
                    return Result<RefreshTokens>.Failure("Token not found.");
                }

                return Result<RefreshTokens>.Success(getToken);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task SaveRefreshTokenAsync(RefreshTokens refreshTokens, CancellationToken cancellationToken)
        {
            await _refreshTokensRepository.SaveAsync(refreshTokens, cancellationToken);
        }

        private RefreshTokens CreateRefreshToken(string userName)
        {
            string generatedToken;
            var randomNumber = new byte[32];

            using (var randonNumberGeneric = RandomNumberGenerator.Create())
            {
                randonNumberGeneric.GetBytes(randomNumber);
                generatedToken = Convert.ToBase64String(randomNumber);
            }

            var token = generatedToken.Replace("+", string.Empty).Replace("=", string.Empty).Replace("/", string.Empty);

            var refreshToken = RefreshTokens.Create(userName, token, _jtwSettings.RefreshTokenExpiration);
            
            return refreshToken;
        }

        private ClaimsIdentity GetClaimsIdentity(UserAdmin userAdmin)
        {
            var identity = new ClaimsIdentity
            (
                new GenericIdentity(userAdmin.Email),
                new[] { 
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, userAdmin.Name)
                }
            );

            return identity;
        }
    }
}
