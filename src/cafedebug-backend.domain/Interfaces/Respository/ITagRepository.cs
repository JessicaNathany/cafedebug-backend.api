using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respositories;

namespace cafedebug_backend.domain.Interfaces.Respository
{
    public interface ITagRepository : IBaseRepository<Tag>
    {
        Task<IEnumerable<Tag>> GetPagedAsync(string searchParam, int pageIndex = 0, int pageSize = 10);
        Task<IEnumerable<Tag>> GetByName(string name);
        Task<IEnumerable<Tag>> GetByEpisodeId(int id);
    }
}
