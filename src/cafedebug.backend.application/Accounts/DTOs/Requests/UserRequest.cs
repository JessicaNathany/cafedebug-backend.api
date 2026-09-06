namespace cafedebug.backend.application.Accounts.DTOs.Requests;

public sealed record UserRequest
{
    public string Name { get; init; }
    public string Email { get; init; }
    public string HashedPassword { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime LastUpdate { get; init; }
}
