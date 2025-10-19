using cafedebug_backend.domain.Episodes;
using cafedebug_backend.domain.Podcasts;

namespace cafedebug.backend.application.Podcasts.DTOs.Requests;

public sealed record CreateEpisodeRequest
{
    public string Title { get; init; }
    public string Description { get; init; }
    public string ShortDescription { get; init; }
    public string Url { get; init; }
    public string ImageUrl { get; init; }
    public List<string>? Tags { get; init; }
    public DateTime PublishedAt { get; init; }
    public bool Active { get; init; }
    public int Number { get; init; }
    public Guid CategoryCode { get; init; }
    public int DurationInSeconds { get; init; }
    
    public Episode ToEpisode()
    {
        return new Episode(
            Title,
            Description,
            ShortDescription,
            Url,
            ImageUrl,
            Tags,
            PublishedAt,
            Active,
            Number,
            DurationInSeconds);
    }
}