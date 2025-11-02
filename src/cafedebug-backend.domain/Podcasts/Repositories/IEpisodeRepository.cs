using cafedebug_backend.domain.Shared.Repositories;

namespace cafedebug_backend.domain.Podcasts.Repositories;

public interface IEpisodeRepository : IBaseRepository<Episode>
{
    Task<IEnumerable<Episode>> GetLastThreeEpisodes();

    Task<IEnumerable<Episode>> SearchByEpisodeName(string searchParam, int pageIndex = 0, int pageSize = 10);

    Task<IEnumerable<Episode>> GetEpisodesPagination(string searchParam, int pageIndex = 0, int pageSize = 10);
}