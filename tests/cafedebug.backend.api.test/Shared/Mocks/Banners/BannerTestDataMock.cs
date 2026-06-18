using AutoFixture;
using cafedebug.backend.application.Banners.DTOs.Requests;
using cafedebug.backend.application.Common.Pagination;
using cafedebug_backend.domain.Banners;
using cafedebug_backend.infrastructure.Data.Pagination;
using MockQueryable;

namespace cafedebug.backend.api.test.Shared.Mocks.Banners;

public class BannerTestDataMock(IFixture fixture)
{
    public BannerRequest CreateBannerRequest()
    {
        return fixture.Build<BannerRequest>()
            .With(r => r.UrlImage, $"https://example.com/{Guid.NewGuid():N}.png")
            .With(r => r.Url, $"https://example.com/{Guid.NewGuid():N}")
            .With(r => r.StartDate, DateTime.UtcNow.AddDays(-1))
            .With(r => r.EndDate, DateTime.UtcNow.AddDays(7))
            .With(r => r.Status, BannerStatus.Published.Value)
            .With(r => r.Active, true)
            .Create();
    }

    public Banner CreateBanner(int bannerId)
    {
        var banner = new Banner(
            fixture.Create<string>(),
            $"https://example.com/{Guid.NewGuid():N}.png",
            $"https://example.com/{Guid.NewGuid():N}",
            DateTime.UtcNow.AddDays(-1),
            DateTime.UtcNow.AddDays(7),
            BannerStatus.Published,
            true,
            fixture.Create<int>());

        banner.Id = bannerId;

        return banner;
    }

    public PageRequest CreatePageRequest()
    {
        return fixture.Build<PageRequest>()
            .With(r => r.Page, 1)
            .With(r => r.PageSize, 10)
            .With(r => r.SortBy, "Title")
            .Create();
    }

    public PagedList<Banner> CreateBannerPagedResult(int page, int pageSize, string? sortBy = null, bool descending = false, int totalCount = 10)
    {
        var banners = Enumerable.Range(1, totalCount)
            .Select(CreateBanner)
            .ToList();

        var bannersQuery = banners.BuildMock();

        return new PagedList<Banner>(bannersQuery, page, pageSize, sortBy, descending);
    }
}
