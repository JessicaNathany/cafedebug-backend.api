using cafedebug_backend.domain.Banners;
using cafedebug_backend.domain.Banners.Repositories;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.infrastructure.Data.Repositories;

public class BannerRepository(CafedebugContext context) : BaseRepository<Banner>(context), IBannerRepository
{
    private readonly CafedebugContext _context = context;
    
    public async Task<Banner?> GetByNameAsync(string bannerName)
    {
        return await _context.Banners
            .AsNoTracking()
            .Where(banner => banner.Name.Contains(bannerName, StringComparison.InvariantCultureIgnoreCase))
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Banner>> GetPagedAsync(int pageIndex = 0, int pageSize = 10)
    {
        var query = _context.Banners
            .AsNoTracking()
            .Where(banner => banner.Active && banner.StartDate.Date <= DateTime.Now.Date && banner.EndDate.Date >= DateTime.Now.Date)
            .Skip(pageIndex * pageSize)
            .Take(pageSize);

        return (IEnumerable<Banner>)await query.ToListAsync();
    }
}