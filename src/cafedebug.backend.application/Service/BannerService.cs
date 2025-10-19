using cafedebug_backend.domain.Banners.Errors;
using cafedebug.backend.application.Validations;
using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Repositories;
using cafedebug_backend.domain.Interfaces.Services;
using cafedebug_backend.domain.Shared;
using Microsoft.Extensions.Logging;

namespace cafedebug.backend.application.Service;

public class BannerService : IBannerService
{
    private readonly IBannerRepository _bannerRepository;
    private readonly ILogger<BannerService> _logger;

    public BannerService(IBannerRepository bannerRepository, ILogger<BannerService> logger)
    {
        _bannerRepository = bannerRepository;
        _logger = logger;
    }

    public async Task<Result<Banner>> CreateAsync(Banner banner)
    {
        var bannerValidator = new BannerValidation();
        var validationResult = bannerValidator.Validate(banner);

        // if (!validationResult.IsValid)
        // {
        //     var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
        //     _logger.LogWarning($"Banner is invalid. {errors}");
        //     return Result.Failure<Banner>(validationResult.Errors[0].ErrorMessage);
        // }

        var bannerExist = await _bannerRepository.GetByNameAsync(banner.Name);

        if (bannerExist != null)
            return Result.Failure<Banner>(BannerError.AlreadyExists);

        await _bannerRepository.SaveAsync(banner);
        _logger.LogInformation($"Banner saved with success.");

        return Result.Success(banner);
    }

    public async Task<Result<Banner>> UpdateAsync(Banner banner)
    {
        // if (banner is null)
        // {
        //     _logger.LogWarning($"Object Banner is cannot be null.");
        //     return Result<Banner>.Failure("Banner cannot be null.");
        // }

        var bannerValidator = new BannerValidation();
        var validationResult = bannerValidator.Validate(banner);

        // if (!validationResult.IsValid)
        // {
        //     var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
        //     _logger.LogWarning($"Banner is invalid. {errors}");
        //     return Result<Banner>.Failure(validationResult.Errors[0].ErrorMessage);
        // }

        var bannerRepository = await _bannerRepository.GetByIdAsync(banner.Id);

        if (bannerRepository is null)
            return Result.Failure<Banner>(BannerError.NotFound(banner.Id));

        banner.Update(
            bannerRepository.Name,
            bannerRepository.UrlImage,
            bannerRepository.Url,
            bannerRepository.StartDate,
            bannerRepository.EndDate,
            bannerRepository.UpdateDate,
            bannerRepository.Active,
            bannerRepository.Ordem);

        await _bannerRepository.UpdateAsync(banner);
        _logger.LogInformation($"Banner updated with success.");

        return Result.Success(banner);
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var banner = await _bannerRepository.GetByIdAsync(id);

        if (banner is null)
        {
            _logger.LogWarning($"Banner not found - banner id: {id}.");
            return Result.Failure(BannerError.NotFound(id));
        }

        await _bannerRepository.DeleteAsync(id);
        _logger.LogInformation($"Banner deleted with success.");

        return Result.Success();
    }

    public async Task<Result<Banner>> GetByIdAsync(int id)
    {
        var banner = await _bannerRepository.GetByIdAsync(id);

        if (banner is null)
        {
            _logger.LogWarning($"Banner not found - banner id: {id}.");
            return Result.Failure<Banner>(BannerError.NotFound(id));
        }

        return Result.Success(banner);
    }

    public async Task<Result<List<Banner>>> GetAllAsync()
    {
        var banners = await _bannerRepository.GetAllAsync();


        return Result.Success(banners.ToList());
    }
}