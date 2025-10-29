using cafedebug_backend.domain.Interfaces.Repositories;
using cafedebug_backend.domain.Interfaces.Respositories;
using cafedebug_backend.domain.Podcasts;
using cafedebug_backend.domain.Shared.Repositories;

namespace cafedebug_backend.domain.Episodes.Repositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<IEnumerable<Category>> GetPagedAsync(string searchParam, int pageIndex = 0, int pageSize = 10);
    }
}
