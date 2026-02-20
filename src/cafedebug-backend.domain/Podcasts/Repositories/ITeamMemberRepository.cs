using cafedebug_backend.domain.Shared.Repositories;

namespace cafedebug_backend.domain.Podcasts.Repositories;

public interface ITeamMemberRepository : IBaseRepository<TeamMember>
{
    Task<IEnumerable<TeamMember>> GetPagedAsync(string searchParam, int pageIndex = 0, int pageSize = 10);

    Task<List<TeamMember>> GetTeamsPage();
}