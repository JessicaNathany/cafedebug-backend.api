﻿using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace cafedebug_backend.domain.Jwt
{
    public class JwtSettings
    {
        public string Audience { get; }
        public string Issuer { get; }
        public int ValidForMinutes { get; }
        public int RefreshTokenValidForMinutes { get; }
        public SigningCredentials SigningCredentials { get; }
        public DateTime IssuedAt => DateTime.UtcNow;
        public DateTime NotBefore => DateTime.UtcNow;
        public DateTime AccessTokenExpiration => IssuedAt.AddMinutes(ValidForMinutes);
        public DateTime RefreshTokenExpiration => IssuedAt.AddMinutes(RefreshTokenValidForMinutes);

        public JwtSettings(IConfiguration configuration)
        {
            Issuer = configuration["JwtSettings:Issuer"];
            Audience = configuration["JwtSettings:Audience"];
            ValidForMinutes = Convert.ToInt32(configuration["JwtSettings:ValidForMinutes"]);
            RefreshTokenValidForMinutes = Convert.ToInt32(configuration["JwtSettings:RefreshTokenValidForMinutes"]);

            var signingKey = configuration["JwtSettings:SigningKey"];
            var symetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)); 
            SigningCredentials = new SigningCredentials(symetricKey, SecurityAlgorithms.HmacSha256);
        }
    }
}
