using cafedebug.backend.application.Banners.DTOs.Requests;
using cafedebug.backend.application.Banners.DTOs.Responses;
using cafedebug.backend.application.Banners.Interfaces;
using cafedebug.backend.application.Common.Mappings;
using cafedebug.backend.application.Common.Pagination;
using cafedebug_backend.domain.Banners;
using cafedebug_backend.domain.Banners.Errors;
using cafedebug_backend.domain.Banners.Repositories;
using cafedebug_backend.domain.Episodes.Errors;
using cafedebug_backend.domain.Shared;

namespace cafedebug.backend.application.Banners.Services;

/// <summary>
/// Service responsible for managing banners, including creation, updating, deletion, and retrieval operations.
/// </summary>
public class BannerService(IBannerRepository bannerRepository) : IBannerService
{
    public async Task<Result<BannerResponse>> CreateAsync(BannerRequest request)
    {
        var banner = request.ToBanner();

        var exists = await bannerRepository.AnyAsync(e => e.Name == banner.Name);

        if (exists)
            return Result.Failure<BannerResponse>(BannerError.AlreadyExists(request.Name));

        await bannerRepository.SaveAsync(banner);

        var response = MappingConfig.ToBanner(banner);
        return Result.Success(response);
    }

    public async Task<Result<BannerResponse>> UpdateAsync(BannerRequest request, int id)
    {
        var banner = await bannerRepository.GetByIdAsync(id);

        if (banner is null)
            return Result.Failure<BannerResponse>(EpisodeError.NotFound(id));

        banner.Update(
            request.Name,
            request.UrlImage,
            request.Url,
            request.StartDate,
            request.EndDate,
            request.UpdateDate,
            request.Active,
            request.Order);

        await bannerRepository.UpdateAsync(banner);

        var response = MappingConfig.ToBanner(banner);
        return Result.Success(response);
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var banner = await bannerRepository.GetByIdAsync(id);

        if (banner is null)
            return Result.Failure<Banner>(BannerError.NotFound(id));

        await bannerRepository.DeleteAsync(banner);
        return Result.Success();
    }

    public async Task<Result<BannerResponse>> GetByIdAsync(int id)
    {
        var banner = await bannerRepository.GetByIdAsync(id);

        if (banner is null)
            return Result.Failure<BannerResponse>(BannerError.NotFound(id));

        var response = MappingConfig.ToBanner(banner); 
        return Result.Success(response);
    }

    public async Task<Result<PagedResult<BannerResponse>>> GetAllAsync(PageRequest request)
    {
        var banners = await bannerRepository.GetPageList(request.Page, request.PageSize, request.SortBy, request.Descending);

        return banners.MapToPagedResult(banner => banner.ToBanner());
    }

    public async Task<Result<BannerResponse>> GetByNameAsync(string name)
    {
        var banner = await bannerRepository.GetByNameAsync(name);

        if (banner is null)
            return Result.Failure<BannerResponse>(BannerError.NotFound(name));

        var response = MappingConfig.ToBanner(banner);
        return Result.Success(response);
    }
}