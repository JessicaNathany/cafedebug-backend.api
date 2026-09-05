namespace cafedebug.backend.application.Accounts.DTOs.Response;

public sealed record AccountResponse
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
    public string CreatedDate { get; init; }
    public string LastUpdate { get; init; }
    public string UpdatedBy { get; init; }
}
