using cafedebug_backend.domain.Shared.Repositories;

namespace cafedebug_backend.domain.Podcasts.Repositories
{
    public interface ITagsRepository : IBaseRepository<Tags>
    {
        Task<Tags?> GetByNameAsync(string categoryName);
        Task<IEnumerable<Tags>> GetPagedAsync(int pageIndex = 0, int pageSize = 10);
    }
}
