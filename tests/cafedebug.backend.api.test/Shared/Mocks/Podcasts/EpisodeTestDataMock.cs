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
        var builder = fixture.Build<EpisodeRequest>()
            .With(r => r.Status, EpisodeStatus.Published.Value);

        return categoryId.HasValue ? builder.With(r => r.CategoryId, categoryId.Value).Create() : builder.Create();
    }
    
    public Episode CreateEpisode(int episodeId)
    {
        var categoryId = fixture.Create<int>();
        var episode = new Episode(
            fixture.Create<string>(),
            fixture.Create<string>(),
            fixture.Create<string>(),
            fixture.Create<Uri>().ToString(),
            fixture.Create<Uri>().ToString(),
            fixture.CreateMany<string>(2).ToList(),
            DateTime.Now.AddHours(-3),
            EpisodeStatus.Published,
            fixture.Create<int>(),
            categoryId);

        episode.Id = episodeId;

        var category = fixture.Build<Category>()
            .With(c => c.Id, categoryId)
            .Create();

        episode.SetCategory(category);

        return episode;
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
      var episodes = Enumerable.Range(1, totalCount)
          .Select(CreateEpisode)
          .ToList();
        
        var episodesQuery = episodes.BuildMock();
        
        return new PagedList<Episode>(episodesQuery, page, pageSize, sortBy, descending);
    }
}