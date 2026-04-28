namespace cafedebug_backend.domain.Podcasts;

public sealed record EpisodeStatus(string Value)
{
    public static readonly EpisodeStatus Draft = new("draft");
    public static readonly EpisodeStatus Scheduled = new("scheduled");
    public static readonly EpisodeStatus Published = new("published");
    public static readonly EpisodeStatus Archived = new("archived");
    
    public override string ToString() => Value;
    
    public static implicit operator string(EpisodeStatus status) => status.Value;
    
    public static implicit operator EpisodeStatus(string status) => status switch
    {
        "draft" => Draft,
        "scheduled" => Scheduled,
        "published" => Published,
        "archived" => Archived,
        _ => throw new ArgumentException("Invalid episode status", nameof(status))
    };
}