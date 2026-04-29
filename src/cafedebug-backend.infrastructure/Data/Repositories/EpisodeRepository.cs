using cafedebug_backend.domain.Podcasts;
using cafedebug_backend.domain.Podcasts.Repositories;
using cafedebug_backend.domain.Shared;
using cafedebug_backend.infrastructure.Data.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace cafedebug_backend.infrastructure.Data.Repositories;

public class EpisodeRepository(CafedebugContext context) : BaseRepository<Episode>(context), IEpisodeRepository
{
    private readonly CafedebugContext _context = context;

    public override async Task<Episode?> GetByIdAsync(int id) 
    {
        return await _context.Episodes
            .Include(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Episode>> GetLastThreeEpisodes()
    {
        return await _context.Episodes.OrderByDescending(x => x.Number).Include(e => e.Category).Take(3).ToListAsync();
    }
    
    public async Task<IPagedResult<Episode>> GetPageList(string? term = null, int page = 1, int pageSize = 10, string? sortBy = null,
        bool descending = false, CancellationToken cancellationToken = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        IQueryable<Episode> query;

        if(!string.IsNullOrWhiteSpace(term))
        {
            query = SearchEpisodesQuery(term);
        }
        else
        {
          query = GetEpisodesQuery(sortBy, descending);
        }

        return await PagedList<Episode>.CreateAsync(query, page, pageSize, sortBy, descending, cancellationToken);
    }

    private IQueryable<Episode> GetEpisodesQuery(string? sortBy, bool descending)
    {
        var query = _context.Set<Episode>()
            .Include(e => e.Category) 
            .OrderByDescending(e=> e.Number)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            query = ApplySorting(query, sortBy, descending);
        }

        return query;
    }


    private IIncludableQueryable<Episode, Category> SearchEpisodesQuery(string term)
    {
        var normalizedSearchParam = term.Trim();

        var query =  _context.Episodes
            .FromSqlInterpolated($"""
                                  SELECT e.*
                                  FROM Episode e
                                  WHERE MATCH(e.Title, e.Description) AGAINST ({normalizedSearchParam} IN NATURAL LANGUAGE MODE)
                                  ORDER BY MATCH(e.Title, e.Description) AGAINST ({normalizedSearchParam} IN NATURAL LANGUAGE MODE) DESC,
                                           e.Number DESC
                                  """)
            .AsNoTracking()
            .Include(e => e.Category);

        return query;
    }
}