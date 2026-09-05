namespace cafedebug.backend.application.Banners.DTOs.Responses;

public sealed record BannerResponse
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string UrlImage { get; init; }
    public string Url { get; init; }
    public string StartDate { get; init; }
    public string EndDate { get; init; }
    public string UpdateDate { get; init; }
    public string Active { get; init; }
    public int Order { get; init; }
}
