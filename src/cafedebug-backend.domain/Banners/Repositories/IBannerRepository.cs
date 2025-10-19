using cafedebug_backend.domain.Interfaces.Repositories;
using cafedebug_backend.domain.Shared.Repositories;

namespace cafedebug_backend.domain.Banners.Repositories;

public interface IBannerRepository : IBaseRepository<Banner>
{
    Task<Banner?> GetByNameAsync(string bannerName);

    Task<IEnumerable<Banner>> GetPagedAsync(int pageIndex = 0, int pageSize = 10);
}