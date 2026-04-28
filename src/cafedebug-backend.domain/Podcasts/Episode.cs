using cafedebug_backend.domain.Shared;
using static cafedebug_backend.domain.Podcasts.EpisodeStatus;

namespace cafedebug_backend.domain.Podcasts;

public class Episode : Entity
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string ShortDescription { get; private set; }
    public string Url { get; private set; }
    public string ImageUrl { get; private set; }
    public IReadOnlyCollection<string>? Tags { get; private set; }
    public DateTime PublishedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    private EpisodeStatus _status;
    public EpisodeStatus Status
    {
        get => GetStatus();
        private set => _status = value;
    }

    public int Number { get; private set; }
    public int CategoryId { get; private set; }
    public Category Category { get; private set; }
    public long Views { get; private set; }
    public long Likes { get; private set; }

    // EF Core will use this constructor 
    private Episode() { }
    public Episode(
        string title,
        string description,
        string shortDescription,
        string url,
        string imageUrl,
        List<string>? tags,
        DateTime publishedAt,
        EpisodeStatus status,
        int number,
        int categoryId)
    {
        Title = title;
        Description = description;
        ShortDescription = shortDescription;
        Url = url;
        ImageUrl = imageUrl;
        Tags = tags?.AsReadOnly();
        PublishedAt = publishedAt;
        Status = status;
        Number = number;
        CategoryId = categoryId;
        Views = 0;
        Likes = 0;
        CreatedAt = DateTime.Now;
    }

    public void Update(
        string title,
        string description,
        string shortDescription,
        string url,
        string imageUrl,
        List<string>? tags,
        DateTime publishedAt,
        EpisodeStatus status,
        int number,
        int categoryId)
    {
       Title = title;
       Description = description;
       ShortDescription = shortDescription;
       Url = url;
       ImageUrl = imageUrl;
       Tags = tags?.AsReadOnly();
       PublishedAt = publishedAt;
       UpdatedAt = DateTime.Now;
       Status = status;
       Number = number;
       CategoryId = categoryId;
    }

    public void SetCategory(Category category)
    {
        Category = category;
    }

    private EpisodeStatus GetStatus()
    {
        return _status == Draft || _status == Archived
            ? _status
            : DateTime.Now > PublishedAt
                ? Published
                : Scheduled;
    }
}