namespace cafedebug.backend.application.Accounts.DTOs.Response;

public sealed record JWTTokenResponse
{
    public string AccessToken { get; init; }
    public RefreshTokenResponse RefreshToken { get; init; }
    public string TokenType { get; init; }
    public long ExpiresIn { get; init; }
}

public sealed record RefreshTokenResponse
{
    public string Token { get; init; }
    public DateTime ExpirationDate { get; init; }
}
