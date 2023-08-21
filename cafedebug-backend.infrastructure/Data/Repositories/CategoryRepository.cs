using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respositories;
using cafedebug_backend.infrastructure.Context;
using cafedebug_backend.infrastructure.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.infrastructure.Data.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(CafedebugContext context) : base(context)
        {
        }

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
}
