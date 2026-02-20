using cafedebug_backend.domain.Podcasts;
using cafedebug_backend.domain.Podcasts.Repositories;
using cafedebug_backend.infrastructure.Data.Pagination;
using cafedebug.backend.application.Common.Pagination;
using Moq;

namespace cafedebug.backend.api.test.Shared.Setups.Podcasts;

public class TeamMemberRepositoryMockSetup(Mock<ITeamMemberRepository> teamMemberRepositoryMock)
{
    public void TeamMemberSave(Action<TeamMember> callback)
    {
        var random = new Random();
        teamMemberRepositoryMock.Setup(x => x.SaveAsync(It.IsAny<TeamMember>()))
            .Callback<TeamMember>(member =>
            {
                member.Id = random.Next(1, 1000);
                callback.Invoke(member);
            })
            .Returns(Task.CompletedTask);
    }

    public void TeamMemberSaveThrows(Exception exception)
    {
        teamMemberRepositoryMock
            .Setup(x => x.SaveAsync(It.IsAny<TeamMember>()))
            .ThrowsAsync(exception);
    }

    public void GetTeamMemberById(TeamMember teamMember)
    {
        teamMemberRepositoryMock
            .Setup(x => x.GetByIdAsync(teamMember.Id))
            .ReturnsAsync(teamMember);
    }

    public void TeamMemberUpdate()
    {
        teamMemberRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<TeamMember>()))
            .Returns(Task.CompletedTask);
    }

    public void GetTeamMemberByIdNotFound(int teamMemberId)
    {
        teamMemberRepositoryMock
            .Setup(x => x.GetByIdAsync(teamMemberId))
            .ReturnsAsync((TeamMember?)null);
    }

    public void TeamMemberDelete()
    {
        teamMemberRepositoryMock
            .Setup(x => x.DeleteAsync(It.IsAny<TeamMember>()))
            .Returns(Task.CompletedTask);
    }

    public void TeamMemberPageList(PagedList<TeamMember> teamMembers, PageRequest request)
    {
        teamMemberRepositoryMock
            .Setup(x => x.GetPageList(request.Page, request.PageSize, request.SortBy, request.Descending, CancellationToken.None))
            .ReturnsAsync(teamMembers);
    }
}