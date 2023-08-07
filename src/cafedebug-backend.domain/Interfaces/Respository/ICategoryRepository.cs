using cafedebug_backend.domain.Entities;

namespace cafedebug_backend.domain.Interfaces.Respositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        //Task<IEnumerable<Banner>> GetPagedAsync(int pageIndex = 0, int pageSize = 10, string searchName);
    }
}
