using AutoMapper;
using cafedebug_backend.domain.Episodes.Repositories;
using cafedebug_backend.domain.Podcasts;
using cafedebug_backend.domain.Podcasts.Errors;
using cafedebug_backend.domain.Podcasts.Repositories;
using cafedebug_backend.domain.Shared;
using cafedebug.backend.application.Common.Mappings;
using cafedebug.backend.application.Common.Pagination;
using cafedebug.backend.application.Podcasts.DTOs.Requests;
using cafedebug.backend.application.Podcasts.DTOs.Responses;
using cafedebug.backend.application.Podcasts.Interfaces.Episodes;

namespace cafedebug.backend.application.Podcasts.Services;

/// <summary>
/// Service responsible for managing episodes, including creation, updating, deletion, and retrieval operations.
/// </summary>
public class EpisodeService(IEpisodeRepository episodeRepository, ICategoryRepository categoryRepository, IMapper mapper)
    : IEpisodeService
{
    /// <summary>
    /// Creates a new episode using the provided request data.
    /// </summary>
    /// <param name="request">The data required to create a new episode.</param>
    /// <returns>A result containing the created episode response or failure information if creation is not successful.</returns>
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

        var response = mapper.Map<EpisodeResponse>(episode);
        return Result.Success(response);
    }

    /// <summary>
    /// Updates an existing episode with the provided data.
    /// </summary>
    /// <param name="id">The unique identifier of the episode to be updated.</param>
    /// <param name="request">The data to update the episode with.</param>
    /// <returns>A result containing the updated episode or failure information if not successful.</returns>
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

        var response = mapper.Map<EpisodeResponse>(episode);
        return Result.Success(response);
    }

    /// <summary>
    /// Deletes an episode by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the episode to delete.</param>
    /// <returns>A result indicating success or failure of the operation.</returns>
    public async Task<Result> DeleteAsync(int id)
    {
        var episode = await episodeRepository.GetByIdAsync(id);
        if (episode is null)
            return Result.Failure<Episode>(EpisodeError.NotFound(id));

        await episodeRepository.DeleteAsync(episode);
        return Result.Success();
    }


    /// <summary>
    /// Retrieves a paginated list of episodes based on the provided pagination request parameters.
    /// </summary>
    /// <param name="request">The pagination parameters including page number and page size.</param>
    /// <returns>A result containing a paginated list of episode responses or failure information if the retrieval is not successful.</returns>
    public async Task<Result<PagedResult<EpisodeResponse>>> GetAllAsync(PageRequest request)
    {
        var episodes = await episodeRepository.GetPageList(request.Page, request.PageSize, request.SortBy, request.Descending);
        
        return mapper.MapToPagedResult<EpisodeResponse>(episodes);
    }

    /// <summary>
    /// Retrieves an episode by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the episode to retrieve.</param>
    /// <returns>A result containing the episode details if found, or an error if not found.</returns>
    public async Task<Result<EpisodeResponse>> GetByIdAsync(int id)
    {
        var episode = await episodeRepository.GetByIdAsync(id);
        if (episode is null)
            return Result.Failure<EpisodeResponse>(EpisodeError.NotFound(id));

        var response = mapper.Map<EpisodeResponse>(episode);
        return Result.Success(response);
    }
}