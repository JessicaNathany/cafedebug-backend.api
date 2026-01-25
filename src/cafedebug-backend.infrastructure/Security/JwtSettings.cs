using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace cafedebug_backend.infrastructure.Security
{
    public class JwtSettings
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int ValidForMinutes { get; set; }
        public int RefreshTokenValidForMinutes { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
        public DateTime IssuedAt => DateTime.UtcNow;
        public DateTime NotBefore => DateTime.UtcNow;
        public DateTime AccessTokenExpiration => IssuedAt.AddMinutes(ValidForMinutes);
        public DateTime RefreshTokenExpiration => IssuedAt.AddMinutes(RefreshTokenValidForMinutes);

        public JwtSettings() {}

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

        // configure SigningCredentials after bind
        public void ConfigureSigningCredentials(string signingKey)
        {
            var symetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            SigningCredentials = new SigningCredentials(symetricKey, SecurityAlgorithms.HmacSha256);
        }
    }
}

