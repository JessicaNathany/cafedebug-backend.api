using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Interfaces.Services
{
    public interface IBannerService 
    {
        Task<Result> CreateAsync(Banner banner, CancellationToken cancellationToken);
        Task<Result> UpdateAsync(Banner banner , CancellationToken cancellationToken);
        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
