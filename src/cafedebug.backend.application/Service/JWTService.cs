using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.JWT;
using cafedebug_backend.domain.Interfaces.Respositories;
using cafedebug_backend.domain.Jwt;
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
        private readonly IRefreshTokensRepository _refreshTokensRepository;

        public JWTService(JwtSettings jwttSettings, IRefreshTokensRepository refreshTokensRepository)
        {
            _jtwSettings = jwttSettings;
            _refreshTokensRepository = refreshTokensRepository;
        }

        public async Task<JWTToken> GenerateToken(UserAdmin userAdmin, CancellationToken cancellationToken)
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
            var createRefreshToken = CreateRefreshToken(userAdmin.Name, cancellationToken);

            return JWTToken.Create(
                accessToken, createRefreshToken, 
                TokenType.Bearer.ToString(), 
                (long)TimeSpan.FromMinutes(_jtwSettings.ValidForMinutes).TotalSeconds);
        }

        public async Task<RefreshTokens> GetByTokenAsync(string token, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public async Task SaveRefreshTokenAsync(RefreshTokens refreshTokens, CancellationToken cancellationToken)
        {
            await _refreshTokensRepository.SaveAsync(refreshTokens, cancellationToken);
        }

        private RefreshTokens CreateRefreshToken(string userName, CancellationToken cancellationToken)
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
