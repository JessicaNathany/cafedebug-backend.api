using cafedebug_backend.domain.Entities;

namespace cafedebug_backend.domain.Interfaces.Respositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<IEnumerable<Category>> GetPagedAsync(string searchParam, int pageIndex = 0, int pageSize = 10);
    }
}
