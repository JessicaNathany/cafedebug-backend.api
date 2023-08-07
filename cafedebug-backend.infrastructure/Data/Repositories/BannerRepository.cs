using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respositories;
using cafedebug_backend.infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.infrastructure.Data.Repository
{
    public class BannerRepository : Repository<Banner>, IBannerRepository
    {
        private readonly CafedebugContext _context;

        public BannerRepository(CafedebugContext context) : base(context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Banner>> GetPagedAsync(int pageIndex = 0, int pageSize = 10)
        {
            var query = _context.Banners.Where(banner => banner.Active
                                               && banner.StartDate.Date <= DateTime.Now.Date
                                               && banner.EndDate.Date >= DateTime.Now.Date)
                                              .Skip(pageIndex * pageSize)
                                              .Take(pageSize);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Banner>> Get()
        {
            return await _context.Banners.Where(banner => banner.Active 
                                          && banner.StartDate.Date <= DateTime.Now.Date 
                                          && banner.EndDate.Date >= DateTime.Now.Date).ToListAsync();
        }
    }
}
