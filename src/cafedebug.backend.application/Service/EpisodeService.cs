using cafedebug.backend.application.Validations;
using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respositories;
using cafedebug_backend.domain.Interfaces.Services;
using cafedebug_backend.domain.Shared;
using Microsoft.Extensions.Logging;

namespace cafedebug.backend.application.Service
{
    /// <summary>
    /// Class responsible for the business rules of the episode frontend
    /// </summary>
    public class EpisodeService : IEpisodeService
    {
        private readonly IEpisodeRepository _episodeRepository;
        private readonly ILogger<EpisodeService> _logger;
        public EpisodeService(IEpisodeRepository episodeRepository, ILogger<EpisodeService> logger)
        {
            _episodeRepository = episodeRepository;
            _logger = logger;
        }

        public async Task<Result<Episode>> CreateAsync(Episode episode)
        {
            if (episode is null)
            {
                _logger.LogWarning($"Episode cannot be null.");
                return Result<Episode>.Failure("Episode cannot be null.");
            }

            var episodeValidator = new EpisodeValidation();
            var validationResult = episodeValidator.Validate(episode);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                _logger.LogWarning($"Episode is invalid. {errors}");
                return Result<Episode>.Failure(validationResult.Errors[0].ErrorMessage);
            }

            try
            {
                var episodeExist = await _episodeRepository.GetByTitle(episode.Title);

                if (episodeExist != null)
                    return Result<Episode>.Failure($"Banner already exists {episode.Title}.");

                await _episodeRepository.SaveAsync(episode);
                _logger.LogInformation($"Banner saved with success.");

                return Result<Episode>.Success(episode);
            }
            catch (Exception exception)
            {
                _logger.LogError($"An unexpected error occurred. {exception}");
                return Result<Episode>.Failure($"An unexpected error occurred. Erro: {exception.Message}");
            }
        }

        public async Task<Result<Episode>> UpdateAsync(Episode episode)
        {
            if (episode is null)
            {
                _logger.LogWarning($"Object Episode is cannot be null.");
                return Result<Episode>.Failure("Episode cannot be null.");
            }

            var episodeValidator = new EpisodeValidation();
            var validationResult = episodeValidator.Validate(episode);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                _logger.LogWarning($"Episode is invalid. {errors}");
                return Result<Episode>.Failure(validationResult.Errors[0].ErrorMessage);
            }
            try
            {
                var episodeRepository = await _episodeRepository.GetByIdAsync(episode.Id);

                if (episodeRepository is null)
                    return Result<Episode>.Failure($"Episode not found {episodeRepository.Id}.");

                episode.Update(
                    episode.Title,
                    episode.Description,
                    episode.ResumeDescription,
                    episode.Url,
                    episode.ImageUrl,
                    episode.PublicationDate,
                    episode.Active,
                    episode.Number,
                    episode.CategoryId,
                    episode.View,
                    episode.Like);

                await _episodeRepository.UpdateAsync(episode);
                _logger.LogInformation($"Banner updated with success.");

                return Result<Episode>.Success(episode);
            }
            catch (Exception exception)
            {
                _logger.LogError($"An unexpected error occurred. {exception}");
                return Result<Episode>.Failure($"An unexpected error occurred. Erro: {exception.Message}");
            }
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var episode = await _episodeRepository.GetByIdAsync(id);

            if (episode is null)
            {
                _logger.LogWarning($"Episode not found - banner id: {id}.");
                return Result.Failure($"Episode not found - banner id: {id}.");
            }

            try
            {
                await _episodeRepository.DeleteAsync(id);
                _logger.LogInformation($"Episode deleted with success.");

                return Result.Success();
            }
            catch (Exception exception)
            {
                _logger.LogError($"An unexpected error occurred {exception}.");
                return Result.Failure($"An unexpected error occurred. Erro: {exception.Message}");
            }
        }

        public async Task<Result<List<Episode>>> GetAllAsync()
        {
            try
            {
                var banners = await _episodeRepository.GetAllAsync();

                if (banners is null)
                {
                    _logger.LogWarning($"Episode not found - banner");
                    return Result<List<Episode>>.Failure($"Episode not found - banner.");
                }

                return Result<List<Episode>>.Success(banners.ToList());

            }
            catch (Exception exception)
            {
                _logger.LogError($"An unexpected error occurred {exception}.");
                return Result<List<Episode>>.Failure($"An unexpected error occurred. Erro: {exception.Message}");
            }
        }

        public async Task<Result<Episode>> GetByIdAsync(int id)
        {
            try
            {
                var episode = await _episodeRepository.GetByIdAsync(id);

                if (episode is null)
                {
                    _logger.LogWarning($"Banner not found - banner id: {id}.");
                    return Result<Episode>.Failure($"Episode not found - banner id: {id}.");
                }

                return Result<Episode>.Success(episode);
            }
            catch (Exception exception)
            {
                _logger.LogError($"An unexpected error occurred {exception}.");
                return Result<Episode>.Failure($"An unexpected error occurred. Erro: {exception.Message}");
            }
        }
    }
}
