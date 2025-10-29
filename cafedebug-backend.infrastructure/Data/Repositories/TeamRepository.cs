using cafedebug_backend.domain.Podcasts;
using cafedebug_backend.domain.Podcasts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.infrastructure.Data.Repositories;

public class TeamRepository(CafedebugContext context) : BaseRepository<Team>(context), ITeamRepository
{
    private readonly CafedebugContext _context = context;

    public async Task<IEnumerable<Team>> GetPagedAsync(string searchParam, int pageIndex = 0, int pageSize = 10)
    {
        var query = _context.Set<Team>()
            .AsNoTracking()
            .Where(category => category.Name.Contains(searchParam))
            .Skip(pageIndex * pageSize)
            .Take(pageSize);

        return await query.ToListAsync();
    }

    public async Task<List<Team>> GetTeamsPage()
    {
        return await _context.Teams.OrderBy(x => x.Name).ToListAsync();
    }
}