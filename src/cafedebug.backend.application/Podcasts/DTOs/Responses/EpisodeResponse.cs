using cafedebug_backend.domain.Podcasts;
using cafedebug.backend.application.Common.Mappings;

namespace cafedebug.backend.application.Podcasts.DTOs.Responses;

public sealed record EpisodeResponse : IMapFrom<Episode>
{
    public int id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ShortDescription { get; set; }
    public string Url { get; set; }
    public string ImageUrl { get; set; }
    public IReadOnlyCollection<string>? Tags { get; set; }
    public DateTime PublishedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Active { get; set; }
    public int Number { get; set; }
    public CategoryResponse category { get; set; }
    public int DurationInSeconds { get; set; }
    public int Views { get; set; }
    public int Likes { get; set; }
}