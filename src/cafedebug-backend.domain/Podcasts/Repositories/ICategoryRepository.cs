using cafedebug_backend.domain.Shared.Repositories;

namespace cafedebug_backend.domain.Podcasts.Repositories;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<Category?> GetByNameAsync(string categoryName);
    Task<IEnumerable<Category>> GetPagedAsync(int pageIndex = 0, int pageSize = 10);
}

