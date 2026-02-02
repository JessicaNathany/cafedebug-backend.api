using cafedebug_backend.domain.Banners;
namespace cafedebug.backend.application.Banners.DTOs.Requests;

public sealed record BannerRequest
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string UrlImage { get; init; }
    public string Url { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public DateTime UpdateAt { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdateDate { get; init; }
    public bool Active { get; init; }
    public int Order { get; init; }

    public Banner ToBanner()
    {
        return new Banner(
            Name,
            UrlImage,
            Url,
            StartDate,
            EndDate,
            Active,
            Order);        
    }
}