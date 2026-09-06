namespace cafedebug.backend.application.Podcasts.DTOs.Responses;

public sealed record CategoryResponse
{
    public int Id { get; init; }
    public string Name { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public DateTime CreatedAt { get; init; }
}
