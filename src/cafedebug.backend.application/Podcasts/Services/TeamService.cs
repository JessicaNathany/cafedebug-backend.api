using cafedebug.backend.application.Common.Pagination;
using cafedebug.backend.application.Podcasts.DTOs.Requests;
using cafedebug.backend.application.Podcasts.DTOs.Responses;
using cafedebug.backend.application.Podcasts.Interfaces.Teams;
using cafedebug_backend.domain.Podcasts.Repositories;
using cafedebug_backend.domain.Shared;

namespace cafedebug.backend.application.Podcasts.Services;

/// <summary>
/// Service responsible for managing team, including creation, updating, deletion, and retrieval operations.
/// </summary>
public class TeamService(ITeamRepository teamRepository) : ITeamService
{
    public Task<Result<TeamResponse>> CreateAsync(TeamRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Result<TeamResponse>> UpdateAsync(int id, TeamRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<PagedResult<TeamResponse>>> GetAllAsync(PageRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Result<TeamResponse>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<TeamResponse>> GetByNameAsync(string name)
    {
        throw new NotImplementedException();
    }
}