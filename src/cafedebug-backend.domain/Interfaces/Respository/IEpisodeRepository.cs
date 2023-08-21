using cafedebug_backend.domain.Entities;

namespace cafedebug_backend.domain.Interfaces.Respositories
{
    public interface IEpisodeRepository : IBaseRepository<Episode>
    {
        Task<IEnumerable<Episode>> GetPagedAsync(string searchParam, int pageIndex = 0, int pageSize = 10);
    }
}
