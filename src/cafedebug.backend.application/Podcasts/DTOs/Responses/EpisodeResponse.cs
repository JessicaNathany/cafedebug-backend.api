namespace cafedebug.backend.application.Podcasts.DTOs.Responses;

public sealed record EpisodeResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ShortDescription { get; set; }
    public string Url { get; set; }
    public string ImageUrl { get; set; }
    public IReadOnlyCollection<string>? Tags { get; set; }
    public DateTime PublishedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; }
    public int Number { get; set; }
    public CategoryResponse Category { get; set; }
    public long Views { get; set; }
    public long Likes { get; set; }
}