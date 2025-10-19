using cafedebug_backend.domain.Episodes;
using cafedebug_backend.domain.Episodes.Repositories;
using cafedebug_backend.domain.Podcasts;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.infrastructure.Data.Repositories;

public class CategoryRepository(CafedebugContext context) : BaseRepository<Category>(context), ICategoryRepository
{
    private readonly CafedebugContext _context = context;

    public async Task<IEnumerable<Category>> GetPagedAsync(string searchParam, int pageIndex = 0, int pageSize = 10)
    {
        var query = _context.Set<Category>()
            .AsNoTracking()
            .Where(category => category.Name.Contains(searchParam))
            .Skip(pageIndex * pageSize)
            .Take(pageSize);

        return await query.ToListAsync();
    }
}