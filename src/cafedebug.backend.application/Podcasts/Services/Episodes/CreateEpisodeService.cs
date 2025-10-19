using AutoMapper;
using cafedebug_backend.domain.Episodes.Errors;
using cafedebug_backend.domain.Episodes.Repositories;
using cafedebug_backend.domain.Shared;
using cafedebug.backend.application.Podcasts.DTOs.Requests;
using cafedebug.backend.application.Podcasts.DTOs.Responses;
using cafedebug.backend.application.Podcasts.Interfaces.Episodes;
using Microsoft.Extensions.Logging;

namespace cafedebug.backend.application.Podcasts.Services.Episodes;

public class CreateEpisodeService(IEpisodeRepository episodeRepository, ICategoryRepository categoryRepository, IMapper mapper, ILogger<EpisodeService> logger)
    : ICreateEpisodeService
{
    public async Task<Result<EpisodeResponse>> Handle(CreateEpisodeRequest request)
    {
        var episode = request.ToEpisode();
        
        var exists = await episodeRepository.AnyAsync(e => e.Title == episode.Title);

        if (exists)
            return Result.Failure<EpisodeResponse>(EpisodeError.AlreadyExists(request.Title));
        
        var category = await categoryRepository.GetByCodeAsync(request.CategoryCode);
        if (category is null)
            return Result.Failure<EpisodeResponse>(CategoryError.NotFound(request.CategoryCode));
        
        episode.AddCategory(category);

        await episodeRepository.SaveAsync(episode);
        logger.LogInformation("Episode was saved with success.");
        
        var response = mapper.Map<EpisodeResponse>(episode);

        return Result.Success(response);
    }
}