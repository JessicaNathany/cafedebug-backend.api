﻿using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respositories;
using cafedebug_backend.infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.infrastructure.Data.Repositories
{
    public class ContactRepository : BaseRepository<Contact>, IContactRepository
    {
        public ContactRepository(CafedebugContext context) : base(context)
        {
        }

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
}
