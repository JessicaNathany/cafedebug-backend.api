using cafedebug_backend.domain.Podcasts;
using cafedebug_backend.domain.Podcasts.Repositories;
using cafedebug_backend.domain.Shared;
using cafedebug_backend.infrastructure.Data.Pagination;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.infrastructure.Data.Repositories;

public class EpisodeRepository(CafedebugContext context) : BaseRepository<Episode>(context), IEpisodeRepository
{
    private readonly CafedebugContext _context = context;

    public override async Task<Episode?> GetByIdAsync(int id) 
    {
        return await _context.Episodes
            .Include(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == id && e.Active);
    }
    
    public async Task<IEnumerable<Episode>> SearchByEpisodeName(string searchParam, int pageIndex = 0, int pageSize = 10)
    {
        var query = _context.Set<Episode>()
            .AsNoTracking()
            .Include(e=> e.Category)
            .Where(episode => episode.Title.Contains(searchParam, StringComparison.OrdinalIgnoreCase) && episode.Active)
            .OrderByDescending(episode => episode.Number)
            .Skip(pageIndex * pageSize)
            .Take(pageSize);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Episode>> GetEpisodesPagination(string searchParam, int pageIndex = 0, int pageSize = 10)
    {
        IQueryable<Episode> episodiesQuery = _context.Episodes
            .AsNoTracking()
            .Include(e => e.Category)
            .OrderByDescending(x => x.Number);

        if (!string.IsNullOrEmpty(searchParam))
            episodiesQuery = episodiesQuery.Where(c => c.Title.Contains(searchParam));

        episodiesQuery = episodiesQuery.Skip(pageIndex * pageSize).Take(pageSize);

        return await episodiesQuery.ToListAsync();
    }

    public async Task<IEnumerable<Episode>> GetLastThreeEpisodes()
    {
        return await _context.Episodes.OrderByDescending(x => x.Number).Include(e => e.Category).Take(3).ToListAsync();
    }
    
    public override async Task<IPagedResult<Episode>> GetPageList(int page = 1, int pageSize = 10, string? sortBy = null,
        bool descending = false, CancellationToken cancellationToken = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.Set<Episode>()
            .Include(e => e.Category) 
            .OrderByDescending(e=> e.Number)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            query = ApplySorting(query, sortBy, descending);
        }

        return await PagedList<Episode>.CreateAsync(query, page, pageSize, sortBy, descending, cancellationToken);
    }
}