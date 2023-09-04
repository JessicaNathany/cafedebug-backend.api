using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respository;
using cafedebug_backend.infrastructure.Context;
using cafedebug_backend.infrastructure.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.infrastructure.Data.Repositories
{
    public class TeamRepository : BaseRepository<Team>, ITeamRepository
    {
        public TeamRepository(CafedebugContext context) : base(context)
        {
        }

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
            return _context.Team.OrderBy(x => x.Name).ToList();
        }
    }
}
