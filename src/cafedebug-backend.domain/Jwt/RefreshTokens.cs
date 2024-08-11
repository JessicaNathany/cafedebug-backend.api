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

        public RefreshTokens(string userName, string token, DateTime expirationDate)
        {
            UserName = userName;
            Token = token;
            ExpirationDate = expirationDate;
            CheckExpirationDateToken();
        }

        public static RefreshTokens Create(string userName, string token, DateTime expirationDate)
        {
            return new RefreshTokens(userName, token, expirationDate);
        }

        public void CheckExpirationDateToken()
        {
            if(DateTime.UtcNow >= ExpirationDate)
            {
                IsActive = false;
            }
            else
            {
                IsActive = true;
            }
        }
    }
}
