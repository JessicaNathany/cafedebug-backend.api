using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.JWT;
using cafedebug_backend.domain.Jwt;
using cafedebug_backend.domian.Jwt;
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

        public JWTService(JwtSettings jwttSettings)
        {
            _jtwSettings = jwttSettings;
        }

        public async Task<JWTToken> GenerateToken(UserAdmin userAdmin)
        {
            #region old code
            //var tokenHandler = new JwtSecurityTokenHandler();

            //var secretKey = _configuration["Jwt:SecretKey"];
            //var issuer = _configuration["Jwt:Issuer"];
            //var audience = _configuration["Jwt:Audience"];

            //var key = Encoding.ASCII.GetBytes(secretKey);

            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new[]
            //    {
            //        new Claim(ClaimTypes.NameIdentifier, userAdmin.Id.ToString()),
            //        new Claim(ClaimTypes.Name, userAdmin.Name),
            //        new Claim(ClaimTypes.Email, userAdmin.Email),
            //        new Claim(ClaimTypes.Hash, userAdmin.HashedPassword)
            //    }),

            //    Expires = DateTime.UtcNow.AddMinutes(5),
            //    Issuer = issuer,
            //    Audience = audience,
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //};

            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //return tokenHandler.WriteToken(token);
            #endregion

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
