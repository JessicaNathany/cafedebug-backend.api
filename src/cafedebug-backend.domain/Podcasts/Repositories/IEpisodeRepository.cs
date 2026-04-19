using cafedebug_backend.domain.Shared;
using cafedebug_backend.domain.Shared.Repositories;

namespace cafedebug_backend.domain.Podcasts.Repositories;

public interface IEpisodeRepository : IBaseRepository<Episode>
{
    Task<IEnumerable<Episode>> GetLastThreeEpisodes();

    Task<IPagedResult<Episode>> GetPageList(string? term = null, int page = 1, int pageSize = 10, string? sortBy = null,
        bool descending = false, CancellationToken cancellationToken = default);
}