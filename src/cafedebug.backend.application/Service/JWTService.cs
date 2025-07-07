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
using System.Text;

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

        public async Task<JWTToken> GenerateAccesTokenAndRefreshtoken(UserAdmin userAdmin)
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
                var createRefreshToken = await CreateRefreshToken(userAdmin.Id, userAdmin.Name);

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

        public async Task<Result<RefreshTokens>> GetByTokenAsync(string token)
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

            var refreshToken = RefreshTokens.Create(userId, userName, token, expirationDate);

            var refreshTokenByUser = await _refreshTokensRepository.GetByTokenByUserIdAsync(userId);

            if(refreshTokenByUser != null)
            {
                refreshTokenByUser.InactiveRefreshToken();
                await _refreshTokensRepository.UpdateAsync(refreshTokenByUser);
            }

            await SaveRefreshToken(refreshToken);

            return refreshToken;
        }

        public async Task<JWTToken> GenerateNewAccessToken(UserAdmin userAdmin, RefreshTokens refreshTokens)
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

                return JWTToken.Create(
                    accessToken, refreshTokens,
                    TokenType.Bearer.ToString(),
                    (long)TimeSpan.FromMinutes(_jtwSettings.ValidForMinutes).TotalSeconds);
            }
            catch (Exception)
            {
                throw;
            }
        }
       
        public async Task UpdateRefreshToken(RefreshTokens oldRefreshTokens, RefreshTokens newRefreshTokens)
        {
            oldRefreshTokens.InactiveRefreshToken();
            await _refreshTokensRepository.UpdateAsync(oldRefreshTokens);

            await SaveRefreshToken(newRefreshTokens);
        }

        private async Task SaveRefreshToken(RefreshTokens refreshToken)
        {
            await _refreshTokensRepository.SaveAsync(refreshToken);
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

        public string GenerateResetToken(int userId)
        {
            throw new NotImplementedException();

            //try
            //{
            //    var tokenHandler = new JwtSecurityTokenHandler();
            //    var key = Encoding.UTF8.GetBytes(_jtwSettings..SigningKey); // corrigir depois isso aqui
            //    var tokenDescriptor = new SecurityTokenDescriptor
            //    {
            //        Subject = new ClaimsIdentity(new[] { new Claim("userId", userId.ToString()) }),
            //        Expires = DateTime.UtcNow.AddMinutes(15), 

            //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            //    };

            //    var token = tokenHandler.CreateToken(tokenDescriptor);
            //    return tokenHandler.WriteToken(token);
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }
        public async Task InvalidateRefreshTokenAsync(RefreshTokens refreshToken)
        {
            refreshToken.InactiveRefreshToken();
            await _refreshTokensRepository.UpdateAsync(refreshToken);
        }
    }
}
