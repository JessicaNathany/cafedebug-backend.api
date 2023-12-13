using cafedebug_backend.domain.Request;
using cafedebug_backend.domain.Response;
using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Interfaces.Services
{
    public interface IBannerService 
    {
        Task<Result<BannerResponse>> CreateAsync(BannerRequest bannerRequest, CancellationToken cancellationToken);
        Task<Result<BannerResponse>> UpdateAsync(BannerRequest bannerRequest , CancellationToken cancellationToken);
        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
