using cafedebug_backend.domain.Podcasts.Errors;
using cafedebug.backend.application.Common.Pagination;
using cafedebug.backend.application.Podcasts.DTOs.Requests;
using cafedebug.backend.application.Podcasts.DTOs.Responses;
using cafedebug.backend.application.Podcasts.Interfaces.Teams;
using cafedebug_backend.domain.Podcasts.Repositories;
using cafedebug_backend.domain.Shared;
using cafedebug.backend.application.Common.Mappings;

namespace cafedebug.backend.application.Podcasts.Services;

/// <summary>
/// Service responsible for managing a team member, including creation, updating, deletion, and retrieval operations.
/// </summary>
public class TeamMemberService(ITeamMemberRepository teamMemberRepository) : ITeamMemberService
{
    public async Task<Result<TeamMemberResponse>> CreateAsync(TeamMemberRequest memberRequest)
    {
        var teamMember = memberRequest.ToTeamMember();

        await teamMemberRepository.SaveAsync(teamMember);

        return TeamMemberResponse.ToResponse(teamMember);
    }

    public async Task<Result<TeamMemberResponse>> UpdateAsync(int id, TeamMemberRequest memberRequest)
    {
        var teamMember = await teamMemberRepository.GetByIdAsync(id);
        if (teamMember is null)
            return Result.Failure<TeamMemberResponse>(TeamMemberError.NotFound(id));

        teamMember.Update(
            memberRequest.Name, 
            memberRequest.Email, 
            memberRequest.Nickname, 
            memberRequest.Bio, 
            memberRequest.PodcastRole, 
            memberRequest.GitHubUrl, 
            memberRequest.InstagramUrl, 
            memberRequest.LinkedInUrl, 
            memberRequest.ProfilePhotoUrl, 
            memberRequest.JobTitle, 
            memberRequest.IsActive, 
            memberRequest.JoinedAt);

        await teamMemberRepository.UpdateAsync(teamMember);

        return TeamMemberResponse.ToResponse(teamMember);
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var teamMember = await teamMemberRepository.GetByIdAsync(id);
        if (teamMember is null)
            return Result.Failure<TeamMemberResponse>(TeamMemberError.NotFound(id));
        
        await teamMemberRepository.DeleteAsync(teamMember);
        return Result.Success();
    }

    public async Task<Result<PagedResult<TeamMemberResponse>>> GetAllAsync(PageRequest request)
    {
        var teamMembers = await teamMemberRepository.GetPageList(request.Page, request.PageSize, request.SortBy, request.Descending);
        
        return teamMembers.MapToPagedResult(TeamMemberResponse.ToResponse);
    }

    public async Task<Result<TeamMemberResponse>> GetByIdAsync(int id)
    {
        var teamMember = await teamMemberRepository.GetByIdAsync(id);
        return teamMember is null ? 
            Result.Failure<TeamMemberResponse>(TeamMemberError.NotFound(id)) : 
            Result.Success(TeamMemberResponse.ToResponse(teamMember));
    }
}