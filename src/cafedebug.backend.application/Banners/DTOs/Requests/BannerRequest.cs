using cafedebug_backend.domain.Banners;
namespace cafedebug.backend.application.Banners.DTOs.Requests;

public sealed record BannerRequest
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string UrlImage { get; set; }

    public string Url { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public DateTime? DateUpdate { get;  set; }
    public bool Active { get; set; }
    public int Order { get; set; }

    public Banner ToBanner()
    {
        return new Banner(
            Name,
            UrlImage,
            Url,
            StartDate,
            EndDate,
            UpdateDate,
            Active,
            Order
            );
    }
}