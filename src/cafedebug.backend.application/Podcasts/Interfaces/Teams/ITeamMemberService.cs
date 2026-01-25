using cafedebug.backend.application.Common.Pagination;
using cafedebug.backend.application.Podcasts.DTOs.Requests;
using cafedebug.backend.application.Podcasts.DTOs.Responses;
using cafedebug_backend.domain.Shared;

namespace cafedebug.backend.application.Podcasts.Interfaces.Teams;

public interface ITeamMemberService 
{
    Task<Result<TeamMemberResponse>> CreateAsync(TeamMemberRequest memberRequest);
    Task<Result<TeamMemberResponse>> UpdateAsync(int id, TeamMemberRequest memberRequest);
    Task<Result> DeleteAsync(int id);
    Task<Result<TeamMemberResponse>> GetByIdAsync(int id);
    Task<Result<PagedResult<TeamMemberResponse>>> GetAllAsync(PageRequest request);
}