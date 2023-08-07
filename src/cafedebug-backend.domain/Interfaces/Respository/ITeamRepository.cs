using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respositories;

namespace cafedebug_backend.domain.Interfaces.Respository
{
    public interface ITeamRepository : IBaseRepository<Team>
    {
        //implementar método de paginação
        //Task<PageResult<Team>> GetPaged(PageRequest page);

        Task<List<Team>> GetTeamsPage();
    }
}
