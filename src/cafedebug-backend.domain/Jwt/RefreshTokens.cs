using cafedebug_backend.domain.Entities;

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
        public bool IsActive { get; private set; }

        public RefreshTokens(int userId, string userName, string token, DateTime expirationDate)
        {
            UserId = userId;
            UserName = userName;
            Token = token;
            IsActive = true;
            ExpirationDate = expirationDate;
        }

        public static RefreshTokens Create(int userId, string userName, string token, DateTime expirationDate)
        {
            return new RefreshTokens(userId, userName, token, expirationDate);
        }

        public void InactiveRefreshToken()
        {
            IsActive = false;
        }
    }
}
