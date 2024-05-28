namespace cafedebug_backend.domain.Jwt
{
    /// <summary>
    /// Entity that represents the refresh token
    /// </summary>
    public class RefreshTokens
    {
        public int TokenId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }

        public RefreshTokens(string userName, string token, DateTime expirationDate)
        {
            UserName = userName;
            Token = token;
            ExpirationDate = expirationDate;
        }

        public static RefreshTokens Create(string userName, string token, DateTime expirationDate)
        {
            return new RefreshTokens(userName, token, expirationDate);
        }
    }
}
