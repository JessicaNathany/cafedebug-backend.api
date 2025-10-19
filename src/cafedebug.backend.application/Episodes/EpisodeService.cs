using cafedebug_backend.domain.Episodes.Entities;
using cafedebug_backend.domain.Episodes.Errors;
using cafedebug_backend.domain.Episodes.Repositories;
using cafedebug_backend.domain.Errors;
using cafedebug_backend.domain.Shared;
using cafedebug.backend.application.Episodes.Interfaces;
using Microsoft.Extensions.Logging;

namespace cafedebug.backend.application.Episodes;

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

    public async Task<Result<Episode>> UpdateAsync(Episode episode)
    {
        var episodeRepository = await _episodeRepository.GetByIdAsync(episode.Id);

        if (episodeRepository is null)
            return Result.Failure<Episode>(EpisodeError.NotFound(episode.Id));

        episode.Update(
            episode.Title,
            episode.Description,
            episode.ShortDescription,
            episode.Url,
            episode.ImageUrl,
            episode.PublishedAt,
            episode.Active,
            episode.Number,
            episode.Views,
            episode.Likes);

        await _episodeRepository.UpdateAsync(episode);
        _logger.LogInformation($"Banner updated with success.");

        return Result<Episode>.Success(episode);
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var episode = await _episodeRepository.GetByIdAsync(id);

        if (episode is null)
        {
            _logger.LogWarning($"Episode not found - banner id: {id}.");
            return Result.Failure<Episode>(EpisodeError.NotFound(id));
        }

        await _episodeRepository.DeleteAsync(id);
        _logger.LogInformation($"Episode deleted with success.");

        return Result.Success();
    }

    public async Task<Result<List<Episode>>> GetAllAsync()
    {
        var banners = await _episodeRepository.GetAllAsync();

        if (banners is null)
            return Result<List<Episode>>.Success(new List<Episode>());

        return Result<List<Episode>>.Success(banners.ToList());
    }

    public async Task<Result<Episode>> GetByIdAsync(int id)
    {
        var episode = await _episodeRepository.GetByIdAsync(id);

        if (episode is null)
        {
            _logger.LogWarning($"Banner not found - banner id: {id}.");
            return Result.Failure<Episode>(EpisodeError.NotFound(id));
        }

        return Result<Episode>.Success(episode);
    }
}