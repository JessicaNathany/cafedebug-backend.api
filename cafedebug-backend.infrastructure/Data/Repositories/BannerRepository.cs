using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respositories;
using cafedebug_backend.infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.infrastructure.Data.Repository
{
    public class BannerRepository : BaseRepository<Banner>, IBannerRepository
    {
        public BannerRepository(CafedebugContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Banner>> GetPagedAsync(int pageIndex = 0, int pageSize = 10)
        {
            var query = _context.Banners
                .AsNoTracking()
                .Where(banner => banner.Active && banner.StartDate.Date <= DateTime.Now.Date && banner.EndDate.Date >= DateTime.Now.Date)
                .Skip(pageIndex * pageSize)
                .Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Banner>> GetAsync()
        {
            return await _context.Set<Banner>()
                                 .Where(banner => banner.Active
                                 && banner.StartDate.Date <= DateTime.Now.Date
                                 && banner.EndDate.Date >= DateTime.Now.Date).ToListAsync();
        }
    }
}
