namespace cafedebug.backend.application.Accounts.DTOs.Requests;

public sealed record UserAdminRequest
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}
