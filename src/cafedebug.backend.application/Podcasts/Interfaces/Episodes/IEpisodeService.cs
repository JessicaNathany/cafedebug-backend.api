using cafedebug_backend.domain.Shared;
using cafedebug.backend.application.Common.Pagination;
using cafedebug.backend.application.Podcasts.DTOs.Requests;
using cafedebug.backend.application.Podcasts.DTOs.Responses;

namespace cafedebug.backend.application.Podcasts.Interfaces.Episodes;

public interface IEpisodeService
{
    Task<Result<EpisodeResponse>> CreateAsync(EpisodeRequest request);
    Task<Result<EpisodeResponse>> UpdateAsync(int id, EpisodeRequest request);
    Task<Result> DeleteAsync(int id);
    Task<Result<EpisodeResponse>> GetByIdAsync(int id);
    Task<Result<PagedResult<EpisodeResponse>>> GetAllAsync(PageRequest request);
}