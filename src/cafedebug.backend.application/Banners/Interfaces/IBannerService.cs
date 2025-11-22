using cafedebug.backend.application.Banners.DTOs.Requests;
using cafedebug.backend.application.Banners.DTOs.Responses;
using cafedebug.backend.application.Common.Pagination;
using cafedebug_backend.domain.Shared;

namespace cafedebug.backend.application.Banners.Interfaces;

public interface IBannerService
{
    Task<Result<BannerResponse>> GetByIdAsync(int idn);
    Task<Result<PagedResult<BannerResponse>>> GetAllAsync(PageRequest request);
    Task<Result<BannerResponse>> CreateAsync(BannerRequest banner);
    Task<Result<BannerResponse>> UpdateAsync(BannerRequest banner, int id);
    Task<Result> DeleteAsync(int id);
    Task<Result<BannerResponse>> GetByNameAsync(string name);
}