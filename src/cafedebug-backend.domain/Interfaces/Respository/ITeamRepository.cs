using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respositories;

namespace cafedebug_backend.domain.Interfaces.Respository
{
    public interface ITeamRepository : IBaseRepository<Team>
    {
        Task<IEnumerable<Team>> GetPagedAsync(string searchParam, int pageIndex = 0, int pageSize = 10);

        Task<List<Team>> GetTeamsPage();
    }
}
