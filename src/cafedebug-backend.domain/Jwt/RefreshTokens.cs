using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Jwt
{
    /// <summary>
    /// Entity that represents the refresh token
    /// </summary>
    public class RefreshTokens : Entity
    {
        public int UserId { get; private set; }
        public string UserName { get; private set; }
        public string Token { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public DateTime CreatedDate { get; private set; } = DateTime.UtcNow;
        public DateTime LastUpdate { get; private set; }

        public RefreshTokens(int userId, string userName, string token, DateTime expirationDate, DateTime lastUpdate)
        {
            UserId = userId;
            UserName = userName;
            Token = token;
            ExpirationDate = expirationDate;
            LastUpdate = lastUpdate;
        }

        public static RefreshTokens Create(int userId, string userName, string token, DateTime expirationDate, DateTime lastUpdate)
        {
            return new RefreshTokens(userId, userName, token, expirationDate, lastUpdate);
        }

        public void UpdateToken(string token, DateTime expirationDate)
        {
            Token = token;
            ExpirationDate = expirationDate;
            LastUpdate = DateTime.UtcNow;
        }
    }
}
    