using AutoFixture;
using cafedebug.backend.application.Banners.DTOs.Requests;
using cafedebug.backend.application.Common.Pagination;
using cafedebug.backend.application.Podcasts.DTOs.Requests;
using cafedebug_backend.domain.Banners;
using cafedebug_backend.domain.Podcasts;
using cafedebug_backend.infrastructure.Data.Pagination;
using MockQueryable;

namespace cafedebug.backend.api.test.Shared.Mocks.Podcasts;

public class CategoryTestDataMock(IFixture fixture)
{
    public Category CreateCategory(int id)
    {
        return fixture.Build<Category>()
            .With(c => c.Id, id)
            .Create();
    }

    public CategoryRequest CreateCategoryRequest()
    {
        var build = fixture.Build<CategoryRequest>();
        return build.Create();
    }

    public PageRequest CreatePageRequest()
    {
        return fixture.Build<PageRequest>()
            .With(r => r.Page, 1)
            .With(r => r.PageSize, 10)
            .With(r => r.SortBy, "Title")
            .Create();
    }
    public PagedList<Category> CreateCategoryPagedResult(int page, int pageSize, string? sortBy = null, bool descending = false, int totalCount = 10)
    {
        var categories = fixture.CreateMany<Category>(totalCount).ToList();

        var categoriesQuery = categories.BuildMock();

        return new PagedList<Category>(categoriesQuery, page, pageSize, sortBy, descending);
    }
}