using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respository;
using cafedebug_backend.infrastructure.Context;
using cafedebug_backend.infrastructure.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.infrastructure.Data.Repositories
{
    public class AdvertisementRepository : BaseRepository<Advertisement>, IAdvertisementRepository
    {
        public AdvertisementRepository(CafedebugContext context) : base(context)
        {
        }

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
}
