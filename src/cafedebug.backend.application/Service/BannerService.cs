using cafedebug.backend.application.Validations;
using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respositories;
using cafedebug_backend.domain.Interfaces.Services;
using cafedebug_backend.domain.Request;
using cafedebug_backend.domain.Response;
using cafedebug_backend.domain.Shared;
using Microsoft.Extensions.Logging;

namespace cafedebug.backend.application.Service
{
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly ILogger<BannerService> _logger;
        public BannerService(IBannerRepository bannerRepository, ILogger<BannerService> logger)
        {
            _bannerRepository = bannerRepository;
            _logger = logger;
        }

        public async Task<Result<BannerResponse>> CreateAsync(BannerRequest bannerRequest, CancellationToken cancellationToken)
        {
            if (bannerRequest is null)
            {
                _logger.LogWarning($"Banner cannot be null.");
                return Result<BannerResponse>.Failure("Banner cannot be null.");
            }

            var bannerValidator = new BannerValidation();
            var validationResult = bannerValidator.Validate(bannerRequest);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                _logger.LogWarning($"Banner is invalid. {errors}");
                return Result<BannerResponse>.Failure(validationResult.Errors[0].ErrorMessage);
            }

            try
            {
                var banner = await _bannerRepository.GetByNameAsync(bannerRequest.Name, cancellationToken);

                if (banner != null)
                    return Result<BannerResponse>.Failure($"Banner already exists {bannerRequest.Name}.");

                var newBanner = new Banner(
                    bannerRequest.Name,
                    bannerRequest.UrlImage,
                    bannerRequest.Url,
                    bannerRequest.StartDate,
                    bannerRequest.EndDate,
                    bannerRequest.Active);

                await _bannerRepository.SaveAsync(newBanner, cancellationToken);
                _logger.LogInformation($"Banner saved with success.");

                var bannerResponse = MapperBannerResponse(newBanner);

                return Result<BannerResponse>.Success(bannerResponse);
            }
            catch (Exception exception)
            {
                _logger.LogError($"An unexpected error occurred. {exception}");
                return Result<BannerResponse>.Failure($"An unexpected error occurred. Erro: {exception.Message}");
                throw;
            }
        }

        public async Task<Result<BannerResponse>> UpdateAsync(BannerRequest bannerRequest, CancellationToken cancellationToken)
        {
            if (bannerRequest is null)
            {
                _logger.LogWarning($"Object Banner is cannot be null.");
                return Result<BannerResponse>.Failure("Banner cannot be null.");
            }

            var bannerValidator = new BannerValidation();
            var validationResult = bannerValidator.Validate(bannerRequest);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                _logger.LogWarning($"Banner is invalid. {errors}");
                return Result<BannerResponse>.Failure(validationResult.Errors[0].ErrorMessage);
            }

            try
            {
                var banner = await _bannerRepository.GetByIdAsync(bannerRequest.Id, cancellationToken);

                if(banner is null)
                    return Result<BannerResponse>.Failure($"Banner not found {bannerRequest.Id}.");

                banner.Update(
                    bannerRequest.Name, 
                    bannerRequest.UrlImage, 
                    bannerRequest.Url,
                    bannerRequest.StartDate,
                    bannerRequest.EndDate,
                    bannerRequest.DateUpdate,
                    bannerRequest.Active);

                await _bannerRepository.UpdateAsync(banner, cancellationToken);
                _logger.LogInformation($"Banner updated with success.");

                var bannerResponse = MapperBannerResponse(banner);

                return Result<BannerResponse>.Success(bannerResponse);
            }
            catch (Exception exception)
            {
                _logger.LogError($"An unexpected error occurred. {exception}");
                return Result<BannerResponse>.Failure($"An unexpected error occurred. Erro: {exception.Message}");
                throw;
            }
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var banner = await _bannerRepository.GetByIdAsync(id, cancellationToken);

            if(banner is null)
            {
                _logger.LogWarning($"Banner not found - banner id: {id}.");
                return Result.Failure($"Banner not found - banner id: {id}.");
            }

            try
            {
                await _bannerRepository.DeleteAsync(id, cancellationToken);
                _logger.LogInformation($"Banner deleted with success.");

                return Result.Success();
            }
            catch (Exception exception)
            {
                _logger.LogError($"An unexpected error occurred {exception}.");
                return Result.Failure($"An unexpected error occurred. Erro: {exception.Message}");
                throw;
            }
        }

        private static BannerResponse MapperBannerResponse(Banner? banner)
        {
            return new BannerResponse
            {
                Id = banner.Id,
                Code = banner.Code,
                Name = banner.Name,
                Active = banner.Active,
                StartDate = banner.StartDate,
                EndDate = banner.EndDate,
                UpdateDate = banner.UpdateDate,
                Url = banner.Url,
                UrlImage = banner.UrlImage
            };
        }
    }
}
