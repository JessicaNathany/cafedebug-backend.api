using cafedebug_backend.domain.Interfaces.Repositories;
using cafedebug_backend.domain.Shared.Repositories;

namespace cafedebug_backend.domain.Podcasts.Repositories;

public interface ITeamRepository : IBaseRepository<Team>
{
    Task<IEnumerable<Team>> GetPagedAsync(string searchParam, int pageIndex = 0, int pageSize = 10);

    Task<List<Team>> GetTeamsPage();
}