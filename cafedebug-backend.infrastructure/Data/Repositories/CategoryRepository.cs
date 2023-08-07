using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respositories;
using cafedebug_backend.infrastructure.Context;
using cafedebug_backend.infrastructure.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.infrastructure.Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly CafedebugContext _context;

        public CategoryRepository(CafedebugContext context) : base(context)
        {
            _context = context;
        }

        //public async Task<IEnumerable<Banner>> GetPagedAsync(int pageIndex = 0, int pageSize = 10, string searchName)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
