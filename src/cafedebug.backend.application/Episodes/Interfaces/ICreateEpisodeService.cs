using cafedebug_backend.domain.Shared;
using cafedebug.backend.application.Episodes.DTOs.Request;
using cafedebug.backend.application.Episodes.DTOs.Responses;

namespace cafedebug.backend.application.Episodes.Interfaces;

public interface ICreateEpisodeService
{
    Task<Result<EpisodeResponse>> Handle(CreateEpisodeRequest request);
}