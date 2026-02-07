using AutoFixture;
using cafedebug_backend.domain.Podcasts;
using cafedebug_backend.infrastructure.Data.Pagination;
using cafedebug.backend.application.Common.Pagination;
using cafedebug.backend.application.Podcasts.DTOs.Requests;
using MockQueryable;

namespace cafedebug.backend.api.test.Shared.Mocks.Podcasts;

public class TeamMemberTestDataMock(IFixture fixture)
{
    public TeamMemberRequest CreateTeamMemberRequest()
    {
        return fixture.Build<TeamMemberRequest>()
            .Create();
    }

    public TeamMember CreateTeamMember(int teamMemberId)
    {
        return fixture.Build<TeamMember>()
            .With(x => x.Id, teamMemberId)
            .Create();
    }
    
    public PageRequest CreatePageRequest()
    {
        return fixture.Build<PageRequest>()
            .With(r => r.Page, 1)
            .With(r => r.PageSize, 10)
            .With(r => r.SortBy, "Title")
            .Create();
    }

    public PagedList<TeamMember> CreateTeamMembers(int page, int pageSize, string? sortBy = null, bool descending = false, int totalCount = 10)
    {
        var teamMembers = fixture.CreateMany<TeamMember>(totalCount).ToList();
        
        var teamsQuery = teamMembers.BuildMock();
        
        return new PagedList<TeamMember>(teamsQuery, page, pageSize, sortBy, descending);
    }
}