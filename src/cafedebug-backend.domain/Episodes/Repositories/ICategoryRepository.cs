using cafedebug_backend.domain.Episodes.Entities;
using cafedebug_backend.domain.Interfaces.Repositories;
using cafedebug_backend.domain.Interfaces.Respositories;

namespace cafedebug_backend.domain.Episodes.Repositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<IEnumerable<Category>> GetPagedAsync(string searchParam, int pageIndex = 0, int pageSize = 10);
    }
}
