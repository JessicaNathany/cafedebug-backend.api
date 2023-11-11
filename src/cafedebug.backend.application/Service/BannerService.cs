using cafedebug.backend.application.Validations;
using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respositories;
using cafedebug_backend.domain.Interfaces.Services;
using cafedebug_backend.domain.Shared;
using Microsoft.Extensions.Logging;
using System;

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

        public async Task<Result> CreateAsync(Banner banner, CancellationToken cancellationToken)
        {
            if (banner is null)
            {
                _logger.LogInformation($"Object Banner is cannot be null.");
                return Result.Failure("Banner cannot be null.");
            }

            var bannerValidator = new BannerValidation();
            var validationResult = bannerValidator.Validate(banner);

            if (!validationResult.IsValid)
            {
                _logger.LogInformation($"Object Banner is invalid.");
                return Result<Banner>.Failure(validationResult.Errors[0].ErrorMessage);
            }

            try
            {
                await _bannerRepository.SaveAsync(banner, cancellationToken);
                _logger.LogInformation($"Banner saved with success.");

                return Result.Success();
            }
            catch (Exception exception)
            {
                _logger.LogError($"An unexpected error occurred. {exception}");
                throw;
            }
        }

        public async Task<Result> UpdateAsync(Banner banner, CancellationToken cancellationToken)
        {
            if (banner is null)
            {
                _logger.LogInformation($"Object Banner is cannot be null.");
                return Result.Failure("Banner cannot be null.");
            }

            var bannerValidator = new BannerValidation();
            var validationResult = bannerValidator.Validate(banner);

            if (!validationResult.IsValid)
            {
                _logger.LogInformation($"Object Banner is invalid.");
                return Result<Banner>.Failure(validationResult.Errors[0].ErrorMessage);
            }

            try
            {
                await _bannerRepository.UpdateAsync(banner, cancellationToken);
                _logger.LogInformation($"Banner updated with success.");

                return Result.Success();
            }
            catch (Exception exception)
            {
                _logger.LogError($"An unexpected error occurred. {exception}");
                throw;
            }
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var banner = await _bannerRepository.GetByIdAsync(id, cancellationToken);

            if(banner is null)
            {
                _logger.LogInformation($"Banner not found.{id}");
                return Result<Banner>.Failure($"Banner not found {id}.");
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
                throw;
            }
        }
    }
}
