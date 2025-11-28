using cafedebug_backend.domain.Audience;
using cafedebug_backend.domain.Audience.Repositories;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.infrastructure.Data.Repositories;

public class ContactRepository(CafedebugContext context) : BaseRepository<Contact>(context), IContactRepository
{
    private readonly CafedebugContext _context = context;

    public async Task<IEnumerable<Contact>> GetPagedAsync(string searchParam, int pageIndex = 0, int pageSize = 10)
    {
        var query = _context.Set<Contact>()
            .AsNoTracking()
            .Where(category => category.Name.Contains(searchParam))
            .Skip(pageIndex * pageSize)
            .Take(pageSize);

        return await query.ToListAsync();
    }
}