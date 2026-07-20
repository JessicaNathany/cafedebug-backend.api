using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Podcasts;

public class TeamMember : Entity
{
    public string Name { get; private set; }
    public string? Email { get; private set; }
    public string? Bio { get; private set; }
    /// <summary>
    /// The member's role in the podcast (e.g., Host, Co-Host, Audio Editor, Image Editor, Producer)
    /// </summary>
    public string PodcastRole { get; private set; }
    public string? GitHubUrl { get; private set; }
    public string? LinkedInUrl { get; private set; }
    public string? ProfilePhotoUrl { get; private set; }
    public string? JobTitle { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    
    // EF Core will use this constructor 
    private TeamMember() { }
    
    public TeamMember(string name, string? email, string? bio, string podcastRole, 
        string? githubUrl, string? imageUrl, string? linkedInUrl, string? profilePhotoUrl, 
        string? jobTitle, bool isActive)
    {
        Name = name;
        Email = email;
        Bio = bio;
        PodcastRole = podcastRole;
        GitHubUrl = githubUrl;
        LinkedInUrl = linkedInUrl;
        ProfilePhotoUrl = profilePhotoUrl;
        JobTitle = jobTitle;
        IsActive = isActive;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(
        string name, 
        string? email, 
        string? bio, 
        string podcastRole, 
        string? gitHubUrl, 
        string? linkedInUrl, 
        string? profilePhotoUrl, 
        string? jobTitle, 
        bool isActive)
    {
        Name = name;
        Email = email;
        Bio = bio;
        PodcastRole = podcastRole;
        GitHubUrl = gitHubUrl;
        LinkedInUrl = linkedInUrl;
        ProfilePhotoUrl = profilePhotoUrl;
        JobTitle = jobTitle;
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
    }
}