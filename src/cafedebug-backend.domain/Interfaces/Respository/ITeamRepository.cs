//using cafedebug.backend.application.Pagination;
using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respositories;

namespace cafedebug_backend.domain.Interfaces.Respository
{
    public interface ITeamRepository : IBaseRepository<Team>
    {
        //Task<PageResult<Team>> GetPaged(PageRequest page);

        Task<List<Team>> GetTeamsPage();
    }
}
