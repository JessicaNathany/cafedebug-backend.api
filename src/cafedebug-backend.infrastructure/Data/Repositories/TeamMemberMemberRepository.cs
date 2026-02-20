using cafedebug_backend.domain.Podcasts;
using cafedebug_backend.domain.Podcasts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.infrastructure.Data.Repositories;

public class TeamMemberMemberRepository(CafedebugContext context) : BaseRepository<TeamMember>(context), ITeamMemberRepository
{
    private readonly CafedebugContext _context = context;

    public async Task<IEnumerable<TeamMember>> GetPagedAsync(string searchParam, int pageIndex = 0, int pageSize = 10)
    {
        var query = _context.Set<TeamMember>()
            .AsNoTracking()
            .Where(category => category.Name.Contains(searchParam))
            .Skip(pageIndex * pageSize)
            .Take(pageSize);

        return await query.ToListAsync();
    }

    public async Task<List<TeamMember>> GetTeamsPage()
    {
        return await _context.TeamMembers.OrderBy(x => x.Name).ToListAsync();
    }
}