using cafedebug_backend.domain.Podcasts;
using cafedebug_backend.domain.Podcasts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.infrastructure.Data.Repositories
{
    public class TagsRepository(CafedebugContext context) : BaseRepository<Tags>(context), ITagsRepository
    {
        private readonly CafedebugContext _context = context;

        public async Task<Tags?> GetByNameAsync(string tagsName)
        {
            return await _context.Tags
            .AsNoTracking()
            .Where(tag => tag.Name.Contains(tagsName, StringComparison.InvariantCultureIgnoreCase))
            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Tags>> GetPagedAsync(int pageIndex = 0, int pageSize = 10)
        {
            var query = _context.Tags
            .AsNoTracking()
            .Skip(pageIndex * pageSize)
            .Take(pageSize);

            return (IEnumerable<Tags>)await query.ToListAsync();
        }
    }
}
