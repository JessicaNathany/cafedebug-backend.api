namespace cafedebug.backend.application.Accounts.DTOs.Response;

public sealed record JWTTokenResponse
{
    public string AccessToken { get; set; }
    public RefreshTokenResponse RefreshToken { get; set; }
    public string TokenType { get; set; }
    public long ExpiresIn { get; set; }
}

public sealed record RefreshTokenResponse
{
    public string Token { get; set; }
    public DateTime ExpirationDate { get; set; }
}
