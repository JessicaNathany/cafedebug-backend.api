using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Podcasts;

public class TeamMember : Entity
{
    public string Name { get; private set; }
    public string? Nickname { get; private set; }
    public string? Email { get; private set; }
    public string? Bio { get; private set; }
    /// <summary>
    /// The member's role in the podcast (e.g., Host, Co-Host, Audio Editor, Image Editor, Producer)
    /// </summary>
    public string PodcastRole { get; private set; }
    public string? GitHubUrl { get; private set; }
    public string? InstagramUrl { get; private set; }
    public string? LinkedInUrl { get; private set; }
    public string ProfilePhotoUrl { get; private set; }
    public string? JobTitle { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime? JoinedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    
    // EF Core will use this constructor 
    private TeamMember() { }
    
    public TeamMember(string name, string? nickname, string? email, string? bio, string podcastRole, 
        string? githubUrl, string? instagramUrl, string? linkedInUrl, string profilePhotoUrl, 
        string? jobTitle, bool isActive, DateTime? joinedAt)
    {
        Name = name;
        Nickname = nickname;
        Email = email;
        Bio = bio;
        PodcastRole = podcastRole;
        GitHubUrl = githubUrl;
        InstagramUrl = instagramUrl;
        LinkedInUrl = linkedInUrl;
        ProfilePhotoUrl = profilePhotoUrl;
        JobTitle = jobTitle;
        IsActive = isActive;
        JoinedAt = joinedAt;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(
        string name, 
        string? email, 
        string? nickname,
        string? bio, 
        string podcastRole, 
        string? gitHubUrl, 
        string? instagramUrl, 
        string? linkedInUrl, 
        string profilePhotoUrl, 
        string? jobTitle, 
        bool isActive, 
        DateTime? joinedAt)
    {
        Name = name;
        Email = email;
        Nickname = nickname;
        Bio = bio;
        PodcastRole = podcastRole;
        GitHubUrl = gitHubUrl;
        InstagramUrl = instagramUrl;
        LinkedInUrl = linkedInUrl;
        ProfilePhotoUrl = profilePhotoUrl;
        JobTitle = jobTitle;
        IsActive = isActive;
        JoinedAt = joinedAt;
        UpdatedAt = DateTime.UtcNow;
    }
}