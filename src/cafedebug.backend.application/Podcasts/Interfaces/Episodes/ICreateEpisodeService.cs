using cafedebug_backend.domain.Shared;
using cafedebug.backend.application.Podcasts.DTOs.Requests;
using cafedebug.backend.application.Podcasts.DTOs.Responses;

namespace cafedebug.backend.application.Podcasts.Interfaces.Episodes;

public interface ICreateEpisodeService
{
    Task<Result<EpisodeResponse>> Handle(CreateEpisodeRequest request);
}