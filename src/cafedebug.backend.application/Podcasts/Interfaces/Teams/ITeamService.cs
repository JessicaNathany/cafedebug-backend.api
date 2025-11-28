using cafedebug.backend.application.Common.Pagination;
using cafedebug.backend.application.Podcasts.DTOs.Requests;
using cafedebug.backend.application.Podcasts.DTOs.Responses;
using cafedebug_backend.domain.Shared;

namespace cafedebug.backend.application.Podcasts.Interfaces.Teams;

public interface ITeamService 
{
    Task<Result<TeamResponse>> CreateAsync(TeamRequest request);
    Task<Result<TeamResponse>> UpdateAsync(int id, TeamRequest request);
    Task<Result> DeleteAsync(int id);
    Task<Result<TeamResponse>> GetByIdAsync(int id);
    Task<Result<TeamResponse>> GetByNameAsync(string name);
    Task<Result<PagedResult<TeamResponse>>> GetAllAsync(PageRequest request);
}