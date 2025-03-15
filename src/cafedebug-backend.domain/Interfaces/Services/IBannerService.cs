using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Interfaces.Services
{
    public interface IBannerService
    {
        Task<Result<Banner>> GetByIdAsync(int idn);
        Task<Result<List<Banner>>> GetAllAsync();
        Task<Result<Banner>> CreateAsync(Banner banner);
        Task<Result<Banner>> UpdateAsync(Banner banner);
        Task<Result> DeleteAsync(int id);
    }
}
