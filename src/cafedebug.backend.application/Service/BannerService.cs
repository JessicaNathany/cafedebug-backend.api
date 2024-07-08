using cafedebug.backend.application.Validations;
using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respositories;
using cafedebug_backend.domain.Interfaces.Services;
using cafedebug_backend.domain.Shared;
using cafedebug_backend.infrastructure.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace cafedebug.backend.application.Service
{
    /// <summary>
    /// Class responsible for the business rules of the banner frontend
    /// </summary>
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly ILogger<BannerService> _logger;
        public BannerService(IBannerRepository bannerRepository, ILogger<BannerService> logger)
        {
            _bannerRepository = bannerRepository;
            _logger = logger;
        }

        public async Task<Result<Banner>> CreateAsync(Banner banner, CancellationToken cancellationToken)
        {
            if (banner is null)
            {
                _logger.LogWarning($"Banner cannot be null.");
                return Result<Banner>.Failure("Banner cannot be null.");
            }

            var bannerValidator = new BannerValidation();
            var validationResult = bannerValidator.Validate(banner);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                _logger.LogWarning($"Banner is invalid. {errors}");
                return Result<Banner>.Failure(validationResult.Errors[0].ErrorMessage);
            }

            try
            {
                var bannerExist = await _bannerRepository.GetByNameAsync(banner.Name, cancellationToken);

                if (bannerExist != null)
                    return Result<Banner>.Failure($"Banner already exists {bannerExist.Name}.");

                await _bannerRepository.SaveAsync(banner, cancellationToken);
                _logger.LogInformation($"Banner saved with success.");

                return Result<Banner>.Success(banner);
            }
            catch (Exception exception)
            {
                _logger.LogError($"An unexpected error occurred. {exception}");
                return Result<Banner>.Failure($"An unexpected error occurred. Erro: {exception.Message}");
            }
        }

        public async Task<Result<Banner>> UpdateAsync(Banner banner, CancellationToken cancellationToken)
        {
            if (banner is null)
            {
                _logger.LogWarning($"Object Banner is cannot be null.");
                return Result<Banner>.Failure("Banner cannot be null.");
            }

            var bannerValidator = new BannerValidation();
            var validationResult = bannerValidator.Validate(banner);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                _logger.LogWarning($"Banner is invalid. {errors}");
                return Result<Banner>.Failure(validationResult.Errors[0].ErrorMessage);
            }

            try
            {
                var bannerRepository = await _bannerRepository.GetByIdAsync(banner.Id, cancellationToken);

                if (bannerRepository is null)
                    return Result<Banner>.Failure($"Banner not found {bannerRepository.Id}.");

                banner.Update(
                    bannerRepository.Name,
                    bannerRepository.UrlImage,
                    bannerRepository.Url,
                    bannerRepository.StartDate,
                    bannerRepository.EndDate,
                    bannerRepository.UpdateDate,
                    bannerRepository.Active);

                await _bannerRepository.UpdateAsync(banner, cancellationToken);
                _logger.LogInformation($"Banner updated with success.");

                return Result<Banner>.Success(banner);
            }
            catch (Exception exception)
            {
                _logger.LogError($"An unexpected error occurred. {exception}");
                return Result<Banner>.Failure($"An unexpected error occurred. Erro: {exception.Message}");
            }
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var banner = await _bannerRepository.GetByIdAsync(id, cancellationToken);

            if (banner is null)
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
            }
        }

        public async Task<Result<Banner>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var banner = await _bannerRepository.GetByIdAsync(id, cancellationToken);

                if (banner is null)
                {
                    _logger.LogWarning($"Banner not found - banner id: {id}.");
                    return Result<Banner>.Failure($"Banner not found - banner id: {id}.");
                }

                return Result<Banner>.Success(banner);
            }
            catch (Exception exception)
            {
                _logger.LogError($"An unexpected error occurred {exception}.");
                return Result<Banner>.Failure($"An unexpected error occurred. Erro: {exception.Message}");
            }
        }

        public async Task<Result<List<Banner>>> GetAllAsync()
        {
            try
            {
                var banners = await _bannerRepository.GetAllAsync();

                if (banners is null)
                {
                    _logger.LogWarning($"Banner not found - banner");
                    return Result<List<Banner>>.Failure($"Banner not found - banner.");
                }

                return Result<List<Banner>>.Success(banners.ToList());

            }
            catch (Exception exception)
            {
                _logger.LogError($"An unexpected error occurred {exception}.");
                return Result<List<Banner>>.Failure($"An unexpected error occurred. Erro: {exception.Message}");
            }
        }
    }
}
