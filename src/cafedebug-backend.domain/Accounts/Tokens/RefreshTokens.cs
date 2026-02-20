using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Accounts.Tokens;

/// <summary>
/// Entity that represents the refresh token
/// </summary>
public class RefreshTokens : Entity
{
    public int UserId { get; private set; }
    public string UserName { get; private set; }
    public string Token { get; private set; }
    public DateTime ExpirationDate { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    public RefreshTokens(int userId, string userName, string token, DateTime expirationDate, DateTime? updatedAt)
    {
        UserId = userId;
        UserName = userName;
        Token = token;
        ExpirationDate = expirationDate;
        UpdatedAt = updatedAt;
    }

    public static RefreshTokens Create(int userId, string userName, string token, DateTime expirationDate, DateTime? updatedAt)
    {
        return new RefreshTokens(userId, userName, token, expirationDate, updatedAt);
    }

    public void UpdateToken(string token, DateTime expirationDate)
    {
        Token = token;
        ExpirationDate = expirationDate;
        UpdatedAt = DateTime.UtcNow;
    }
}