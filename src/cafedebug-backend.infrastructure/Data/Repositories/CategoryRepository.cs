using cafedebug_backend.domain.Banners;
using cafedebug_backend.domain.Podcasts;
using cafedebug_backend.domain.Podcasts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.infrastructure.Data.Repositories;

public class CategoryRepository(CafedebugContext context) : BaseRepository<Category>(context), ICategoryRepository
{
    private readonly CafedebugContext _context = context;

    public async Task<Category?> GetByNameAsync(string categoryName)
    {
        return await _context.Categories
            .AsNoTracking()
            .Where(category => category.Name.Contains(categoryName, StringComparison.InvariantCultureIgnoreCase))
            .FirstOrDefaultAsync();
    }
    
    public async Task<IEnumerable<Category>> GetPagedAsync(int pageIndex = 0, int pageSize = 10)
    {
        var query = _context.Categories
            .AsNoTracking()
            .Skip(pageIndex * pageSize)
            .Take(pageSize);

        return (IEnumerable<Category>)await query.ToListAsync();
    }
}