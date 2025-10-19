using cafedebug_backend.domain.Episodes;
using cafedebug_backend.domain.Episodes.Repositories;
using cafedebug_backend.domain.Podcasts;
using cafedebug_backend.infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.infrastructure.Data.Repositories;

public class EpisodeRepository(CafedebugContext context) : BaseRepository<Episode>(context), IEpisodeRepository
{
    private readonly CafedebugContext _context = context;

    public async Task<IEnumerable<Episode>> GetPagedAsync(string searchParam, int pageIndex = 0, int pageSize = 10)
    {
        var query = _context.Set<Episode>()
            .AsNoTracking()
            .Where(category => category.Title.Contains(searchParam))
            .Skip(pageIndex * pageSize)
            .Take(pageSize);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Episode>> SearchByEpisodeName(string searchParam, int pageIndex = 0, int pageSize = 10)
    {
        var query = _context.Set<Episode>()
            .AsNoTracking()
            .Where(episode => episode.Title.Contains(searchParam, StringComparison.OrdinalIgnoreCase) && episode.Active)
            .OrderByDescending(episode => episode.Number)
            .Skip(pageIndex * pageSize)
            .Take(pageSize);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Episode>> GetEpisodesPagination(string searchParam, int pageIndex = 0, int pageSize = 10)
    {
        IQueryable<Episode> episodiesQuery = _context.Episodes.AsNoTracking().OrderByDescending(x => x.Number);

        if (!string.IsNullOrEmpty(searchParam))
            episodiesQuery = episodiesQuery.Where(c => c.Title.Contains(searchParam));

        episodiesQuery = episodiesQuery.Skip(pageIndex * pageSize).Take(pageSize);

        return await episodiesQuery.ToListAsync();
    }

    public async Task<IEnumerable<Episode>> GetLastThreeEpisodes()
    {
        return await _context.Episodes.OrderByDescending(x => x.Number).Take(3).ToListAsync();
    }
}