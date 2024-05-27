using cafedebug_backend.domian.Jwt;

namespace cafedebug_backend.domain.Jwt
{
    public class JWTToken
    {
        public string AccessToken { get; }
        public RefreshToken RefreshToken { get; }
        public string TokenType { get; }
        public long ExpiresIn { get; }

        public JWTToken(string accessToken, RefreshToken refreshToken, string tokenType, long expiresIn)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            TokenType = tokenType;  
            ExpiresIn = expiresIn;
        }

        public static JWTToken Create(string accessToken, RefreshToken refreshToken, string tokenType, long expiresIn)
        {
            JWTToken jwtToken = new(accessToken, refreshToken, tokenType, expiresIn);
            return jwtToken;
        }
    }
}
