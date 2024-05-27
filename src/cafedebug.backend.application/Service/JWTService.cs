using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.JWT;
using cafedebug_backend.domain.Jwt;
using cafedebug_backend.domian.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;

namespace cafedebug.backend.application.Service
{
    public class JWTService : IJWTService
    {
        private readonly JwtSettings _jtwSettings;

        public JWTService(JwtSettings jwttSettings)
        {
            _jtwSettings = jwttSettings;
        }

        public async Task<JWTToken> GenerateToken(UserAdmin userAdmin)
        {
            var identity = GetClaimsIdentity(userAdmin);

            var jsonSecurityHandler = new JwtSecurityTokenHandler();

            var securityToken = jsonSecurityHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = identity,
                Issuer = _jtwSettings.Issuer,
                Audience = _jtwSettings.Audience,
                Expires = _jtwSettings.AccessTokenExpiration,
                SigningCredentials = _jtwSettings.SigningCredentials
            });

            var accessToken = jsonSecurityHandler.WriteToken(securityToken);

            return JWTToken.Create(
                accessToken, CreateRefreshToken(userAdmin.Name), 
                TokenType.Bearer.ToString(), 
                (long)TimeSpan.FromMinutes(_jtwSettings.ValidForMinutes).TotalSeconds);
        }

        public async Task RefreshToken()
        {
            throw new NotImplementedException();
        }

        private RefreshToken CreateRefreshToken(string userName)
        {
            string generatedToken;
            var randomNumber = new byte[32];

            using (var randonNumberGeneric = RandomNumberGenerator.Create())
            {
                randonNumberGeneric.GetBytes(randomNumber);
                generatedToken = Convert.ToBase64String(randomNumber);
            }

            var token = generatedToken.Replace("+", string.Empty).Replace("=", string.Empty).Replace("/", string.Empty);

            var refreshToken = RefreshToken.Create(userName, token, _jtwSettings.RefreshTokenExpiration);

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
