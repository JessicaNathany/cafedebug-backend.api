using cafedebug_backend.domain.Podcasts;
using cafedebug_backend.domain.Shared.Repositories;

namespace cafedebug_backend.domain.Episodes.Repositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<Category?> GetByNameAsync(string categoryName);
        Task<IEnumerable<Category>> GetPagedAsync(int pageIndex = 0, int pageSize = 10);
    }
}
