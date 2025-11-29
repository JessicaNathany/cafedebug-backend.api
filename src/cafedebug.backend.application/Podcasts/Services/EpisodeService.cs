using cafedebug.backend.application.Common.Mappings;
using cafedebug.backend.application.Common.Pagination;
using cafedebug.backend.application.Podcasts.DTOs.Requests;
using cafedebug.backend.application.Podcasts.DTOs.Responses;
using cafedebug.backend.application.Podcasts.Interfaces.Episodes;
using cafedebug_backend.domain.Episodes.Repositories;
using cafedebug_backend.domain.Podcasts;
using cafedebug_backend.domain.Podcasts.Errors;
using cafedebug_backend.domain.Podcasts.Repositories;
using cafedebug_backend.domain.Shared;

namespace cafedebug.backend.application.Podcasts.Services;

/// <summary>
/// Service responsible for managing episodes, including creation, updating, deletion, and retrieval operations.
/// </summary>
public class EpisodeService(IEpisodeRepository episodeRepository, ICategoryRepository categoryRepository)
    : IEpisodeService
{
    public async Task<Result<EpisodeResponse>> CreateAsync(EpisodeRequest request)
    {
        var episode = request.ToEpisode();

        var exists = await episodeRepository.AnyAsync(e => e.Title == episode.Title);

        if (exists)
            return Result.Failure<EpisodeResponse>(EpisodeError.AlreadyExists(request.Title));

        var category = await categoryRepository.GetByIdAsync(episode.CategoryId);
        if (category is null)
            return Result.Failure<EpisodeResponse>(CategoryError.NotFound(request.CategoryId));
        
        episode.SetCategory(category);

        await episodeRepository.SaveAsync(episode);

        var response = MappingConfig.ToEpisode(episode);
        return Result.Success(response);
    }

    public async Task<Result<EpisodeResponse>> UpdateAsync(int id, EpisodeRequest request)
    {
        var episode = await episodeRepository.GetByIdAsync(id);
        if (episode is null)
            return Result.Failure<EpisodeResponse>(EpisodeError.NotFound(id));

        var category = await categoryRepository.GetByIdAsync(request.CategoryId);
        if (category is null)
            return Result.Failure<EpisodeResponse>(CategoryError.NotFound(request.CategoryId));

        episode.Update(
            request.Title,
            request.Description,
            request.ShortDescription,
            request.Url,
            request.ImageUrl,
            request.Tags,
            request.PublishedAt,
            request.Active,
            request.Number,
            request.CategoryId,
            request.DurationInSeconds);
        
        episode.SetCategory(category);
        
        await episodeRepository.UpdateAsync(episode);

        var response = MappingConfig.ToEpisode(episode);
        return Result.Success(response);
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var episode = await episodeRepository.GetByIdAsync(id);
        if (episode is null)
            return Result.Failure<Episode>(EpisodeError.NotFound(id));

        await episodeRepository.DeleteAsync(episode);
        return Result.Success();
    }

    public async Task<Result<PagedResult<EpisodeResponse>>> GetAllAsync(PageRequest request)
    {
        var episodes = await episodeRepository.GetPageList(request.Page, request.PageSize, request.SortBy, request.Descending);
        
        return episodes.MapToPagedResult(episodes => episodes.ToEpisode());
    }

    public async Task<Result<EpisodeResponse>> GetByIdAsync(int id)
    {
        var episode = await episodeRepository.GetByIdAsync(id);
        if (episode is null)
            return Result.Failure<EpisodeResponse>(EpisodeError.NotFound(id));

        var response = MappingConfig.ToEpisode(episode);
        
        return Result.Success(response);
    }
}