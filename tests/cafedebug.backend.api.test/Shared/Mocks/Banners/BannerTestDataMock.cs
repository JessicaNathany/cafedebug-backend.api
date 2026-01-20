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
        var build = fixture.Build<BannerRequest>();
        return build.Create();
    }

    public Banner CreateBanner(int bannerId)
    {
        return fixture.Build<Banner>()
            .With(b => b.Id, bannerId)
            .Create();
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
        var banners = fixture.CreateMany<Banner>(totalCount).ToList();

        var bannersQuery = banners.BuildMock();

        return new PagedList<Banner>(bannersQuery, page, pageSize, sortBy, descending);
    }
}

