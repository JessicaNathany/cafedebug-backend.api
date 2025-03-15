using cafedebug_backend.domain.Entities;

namespace cafedebug_backend.domain.Interfaces.Respositories
{
    public interface IBannerRepository : IBaseRepository<Banner>
    {
        Task<Banner> GetByNameAsync(string bannerName);

        Task<IEnumerable<Banner>> GetPagedAsync(int pageIndex = 0, int pageSize = 10);
    }
}
