using cafedebug_backend.domain.Advertisements;
using cafedebug_backend.domain.Advertisements.Repositories;
using cafedebug_backend.domain.Interfaces.Repositories;
using cafedebug_backend.infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.infrastructure.Data.Repositories;

public class AdvertisementRepository(CafedebugContext context)
    : BaseRepository<Advertisement>(context), IAdvertisementRepository
{
    private readonly CafedebugContext _context = context;

    public async Task<IEnumerable<Advertisement>> GetPagedAsync(string searchParam, int pageIndex = 0, int pageSize = 10)
    {
        var query = _context.Set<Advertisement>()
            .AsNoTracking()
            .Where(category => category.Title.Contains(searchParam))
            .Skip(pageIndex * pageSize)
            .Take(pageSize);

        return await query.ToListAsync();
    }
}