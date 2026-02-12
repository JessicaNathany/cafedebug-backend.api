using cafedebug_backend.domain.Podcasts;

namespace cafedebug.backend.application.Podcasts.DTOs.Requests;

public sealed record TeamMemberRequest
{
    public required string Name { get; init; }
    public string? Nickname { get; init; }
    public string? Email { get; init; }
    public string? Bio { get; init; }
    public required string PodcastRole { get; init; }
    public string? GitHubUrl { get; init; }
    public string? InstagramUrl { get; init; }
    public string? LinkedInUrl { get; init; }
    public string? ProfilePhotoUrl { get; init; }
    public string? JobTitle { get; init; }
    public DateTime? JoinedAt { get; init; }
    public bool IsActive { get; init; } = true;

    public TeamMember ToTeamMember()
    {
        return new TeamMember(
            Name,
            Nickname,
            Email,
            Bio,
            PodcastRole,
            GitHubUrl,
            InstagramUrl,
            LinkedInUrl,
            ProfilePhotoUrl,
            JobTitle,
            IsActive,
            JoinedAt
            );
    }
}