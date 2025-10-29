using cafedebug_backend.domain.Shared;

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
    public bool Active { get; private set; }
    public int Number { get; private set; }
    public int CategoryId { get; private set; }
    public Category Category { get; private set; }
    public int DurationInSeconds { get; private set; }
    public int Views { get; private set; }
    public int Likes { get; private set; }

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
        bool active,
        int number,
        int categoryId,
        int durationInSeconds)
    {
        Title = title;
        Description = description;
        ShortDescription = shortDescription;
        Url = url;
        ImageUrl = imageUrl;
        Tags = tags?.AsReadOnly();
        PublishedAt = publishedAt;
        Active = active;
        Number = number;
        CategoryId = categoryId;
        DurationInSeconds = durationInSeconds;
        EndDateVerify(PublishedAt);
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
        bool active,
        int number,
        int categoryId,
        int durationInSeconds)
    {
       Title = title;
       Description = description;
       ShortDescription = shortDescription;
       Url = url;
       ImageUrl = imageUrl;
       Tags = tags?.AsReadOnly();
       PublishedAt = publishedAt;
       Active = active;
       Number = number;
       CategoryId = categoryId;
       DurationInSeconds = durationInSeconds;
    }

    private void EndDateVerify(DateTime publishedAt)
    {
        if (publishedAt == DateTime.Now.AddDays(-1))
            Active = false;
    }
}