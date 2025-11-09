using AutoFixture;
using cafedebug_backend.domain.Podcasts;
using cafedebug_backend.infrastructure.Data.Pagination;
using cafedebug.backend.application.Common.Pagination;
using cafedebug.backend.application.Podcasts.DTOs.Requests;
using MockQueryable;

namespace cafedebug.backend.api.test.Shared.Mocks.Podcasts;

public class EpisodeTestDataMock(IFixture fixture)
{
    public EpisodeRequest CreateEpisodeRequest(int? categoryId = null)
    {
        var builder = fixture.Build<EpisodeRequest>();

        return categoryId.HasValue ? builder.With(r => r.CategoryId, categoryId.Value).Create() : builder.Create();
    }
    
    public Episode CreateEpisode(int episodeId)
    {
        return fixture.Build<Episode>()
            .With(e => e.Id, episodeId)
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
    
    public PagedList<Episode> CreateEpisodePagedResult(int page, int pageSize, string? sortBy = null, bool descending = false, int totalCount = 10)
    {
        var episodes = fixture.CreateMany<Episode>(totalCount).ToList();
        
        var episodesQuery = episodes.BuildMock();
        
        return new PagedList<Episode>(episodesQuery, page, pageSize, sortBy, descending);
    }
}