using cafedebug_backend.domain.Entities;

namespace cafedebug_backend.domain.Interfaces.Respositories
{
    public interface ITeamRepository : IBaseRepository<Team>
    {
        Task<IEnumerable<Team>> GetPagedAsync(string searchParam, int pageIndex = 0, int pageSize = 10);

        Task<List<Team>> GetTeamsPage();
    }
}
