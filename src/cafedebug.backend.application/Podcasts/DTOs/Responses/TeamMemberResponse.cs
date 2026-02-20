using cafedebug_backend.domain.Podcasts;

namespace cafedebug.backend.application.Podcasts.DTOs.Responses;

public sealed record TeamMemberResponse
{
    public int Id { get; init; }
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
    public bool IsActive { get; init; }
    public DateTime? JoinedAt { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }

    public static TeamMemberResponse ToResponse(TeamMember teamMember)
    {
        return new TeamMemberResponse
        {
            Id = teamMember.Id,
            Name = teamMember.Name,
            Nickname = teamMember.Nickname,
            Email = teamMember.Email,
            Bio = teamMember.Bio,
            PodcastRole = teamMember.PodcastRole,
            GitHubUrl = teamMember.GitHubUrl,
            InstagramUrl = teamMember.InstagramUrl,
            LinkedInUrl = teamMember.LinkedInUrl,
            ProfilePhotoUrl = teamMember.ProfilePhotoUrl,
            JobTitle = teamMember.JobTitle,
            IsActive = teamMember.IsActive,
            JoinedAt = teamMember.JoinedAt,
            CreatedAt = teamMember.CreatedAt,
            UpdatedAt = teamMember.UpdatedAt
        };
    }
}