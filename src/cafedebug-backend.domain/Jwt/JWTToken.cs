namespace cafedebug_backend.domain.Jwt
{
    public class JWTToken
    {
        public string AccessToken { get; }
        public RefreshTokens RefreshToken { get; }
        public string TokenType { get; }
        public long ExpiresIn { get; }

        public JWTToken(string accessToken, RefreshTokens refreshToken, string tokenType, long expiresIn)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            TokenType = tokenType;  
            ExpiresIn = expiresIn;
        }

        public static JWTToken Create(string accessToken, RefreshTokens refreshToken, string tokenType, long expiresIn)
        {
            JWTToken jwtToken = new(accessToken, refreshToken, tokenType, expiresIn);
            return jwtToken;
        }
    }
}
