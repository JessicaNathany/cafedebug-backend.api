namespace cafedebug.backend.application.Accounts.DTOs.Response;

public sealed record UserAdminResponse
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
    public string HashedPassword { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime LastUpdate { get; init; }
}
