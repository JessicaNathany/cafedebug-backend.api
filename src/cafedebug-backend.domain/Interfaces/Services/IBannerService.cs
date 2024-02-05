using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Interfaces.Services
{
    public interface IBannerService 
    {
        Task<Result<Banner>> GetById(int id, CancellationToken cancellationToken);
        Task<Result<List<Banner>>> GetAll();
        Task<Result<Banner>> CreateAsync(Banner banner, CancellationToken cancellationToken);
        Task<Result<Banner>> UpdateAsync(Banner banner, CancellationToken cancellationToken);
        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
