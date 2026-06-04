namespace cafedebug_backend.domain.Banners;
public sealed record BannerStatus(string Value)
{
    public static readonly BannerStatus Draft = new("draft");
    public static readonly BannerStatus Published = new("published");
    public static readonly BannerStatus Archived = new("archived");
    public static readonly BannerStatus Scheduled = new("scheduled");
    public override string ToString() => Value;

    public static implicit operator string(BannerStatus status) => status.Value;

    public static implicit operator BannerStatus(string status) => status switch
    {
        "draft" => Draft,
        "published" => Published,
        "archived" => Archived,
        "scheduled" => Scheduled,
        _ => throw new ArgumentException("Invalid banner status", nameof(status))
    };
}
