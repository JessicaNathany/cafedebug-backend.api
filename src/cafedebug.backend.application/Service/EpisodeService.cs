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

        public async Task<Result<Episode>> CreateAsync(Episode episode, CancellationToken cancellationToken)
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
                var episodeExist = await _episodeRepository.GetByTitle(episode.Title, cancellationToken);

                if (episodeExist != null)
                    return Result<Episode>.Failure($"Banner already exists {episode.Title}.");

                await _episodeRepository.SaveAsync(episode, cancellationToken);
                _logger.LogInformation($"Banner saved with success.");

                return Result<Episode>.Success(episode);
            }
            catch (Exception exception)
            {
                _logger.LogError($"An unexpected error occurred. {exception}");
                return Result<Episode>.Failure($"An unexpected error occurred. Erro: {exception.Message}");
                throw;
            }
        }

        public Task<Result<Episode>> UpdateAsync(Episode episodeRequest)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<Episode>>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Result<Episode>> GetById(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Episode>> UpdateAsync(Episode episodeRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
