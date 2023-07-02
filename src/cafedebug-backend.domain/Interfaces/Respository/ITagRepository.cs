using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respositories;

namespace cafedebug_backend.domain.Interfaces.Respository
{
    public interface ITagRepository : IBaseRepository<Tag>
    {
       // Task<PageResult<Tag>> GetPaged(PageRequest page);
        Task<IEnumerable<Tag>> GetByName(string name);
        Task<IEnumerable<Tag>> GetByEpisodeId(int id);
    }
}
