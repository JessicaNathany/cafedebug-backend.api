using cafedebug_backend.domain.Podcasts;
using cafedebug_backend.domain.Podcasts.Repositories;
using cafedebug_backend.domain.Shared.Errors;
using cafedebug.backend.api.test.Shared;
using cafedebug.backend.api.test.Shared.Mocks.Podcasts;
using cafedebug.backend.api.test.Shared.Setups.Podcasts;
using cafedebug.backend.application.Podcasts.Services;
using Moq;
using Shouldly;
using Xunit;

namespace cafedebug.backend.api.test.Application.Podcasts.Services;

[Collection("PodcastTests")]
public class TeamMemberServiceTest : BaseTest
{
    private readonly TeamMemberService _teamMemberService;

    private readonly Mock<ITeamMemberRepository> _teamMemberRepositoryMock;
    private readonly TeamMemberTestDataMock _teamMemberTestDataMock;
    private readonly TeamMemberRepositoryMockSetup _teamMemberRepositoryMockSetup;

    public TeamMemberServiceTest()
    {
        _teamMemberRepositoryMock = new Mock<ITeamMemberRepository>();
        _teamMemberTestDataMock = new TeamMemberTestDataMock(Fixture);

        _teamMemberService = new TeamMemberService(_teamMemberRepositoryMock.Object);

        _teamMemberRepositoryMockSetup = new TeamMemberRepositoryMockSetup(_teamMemberRepositoryMock);
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var request = _teamMemberTestDataMock.CreateTeamMemberRequest();

        TeamMember? savedTeamMember = null;
        _teamMemberRepositoryMockSetup.TeamMemberSave(t => savedTeamMember = t);

        // Act
        var result = await _teamMemberService.CreateAsync(request);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();

        var response = result.Value;
        response.Name.ShouldBe(request.Name);
        response.Email.ShouldBe(request.Email);
        response.Bio.ShouldBe(request.Bio);
        savedTeamMember.ShouldNotBeNull();
        response.Id.ShouldBe(savedTeamMember.Id);

        _teamMemberRepositoryMock.Verify(x => x.SaveAsync(It.IsAny<TeamMember>()), Times.Once());
    }

    [Fact]
    public async Task CreateAsync_WhenSaveThrowsException_PropagatesException()
    {
        // Arrange
        var request = _teamMemberTestDataMock.CreateTeamMemberRequest();
        var expectedException = new Exception("DB down");

        _teamMemberRepositoryMockSetup.TeamMemberSaveThrows(expectedException);

        // Act
        var act = async () => await _teamMemberService.CreateAsync(request);

        // Assert
        (await act.ShouldThrowAsync<Exception>()).Message.ShouldBe("DB down");

        _teamMemberRepositoryMock.Verify(x => x.SaveAsync(It.IsAny<TeamMember>()), Times.Once());
    }

    [Fact]
    public async Task UpdateAsync_WithValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        const int teamMemberId = 123;
        var teamMember = _teamMemberTestDataMock.CreateTeamMember(teamMemberId);
        var request = _teamMemberTestDataMock.CreateTeamMemberRequest();

        _teamMemberRepositoryMockSetup.GetTeamMemberById(teamMember);
        _teamMemberRepositoryMockSetup.TeamMemberUpdate();

        // Act
        var result = await _teamMemberService.UpdateAsync(teamMemberId, request);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();

        _teamMemberRepositoryMock.Verify(x => x.GetByIdAsync(teamMemberId), Times.Once());
        _teamMemberRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<TeamMember>()), Times.Once());
    }

    [Fact]
    public async Task UpdateAsync_WhenTeamMemberNotFound_ReturnsResourceNotFoundError()
    {
        // Arrange
        const int teamMemberId = 123;
        var request = _teamMemberTestDataMock.CreateTeamMemberRequest();

        _teamMemberRepositoryMockSetup.GetTeamMemberByIdNotFound(teamMemberId);

        // Act
        var result = await _teamMemberService.UpdateAsync(teamMemberId, request);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(nameof(ErrorType.ResourceNotFound));

        _teamMemberRepositoryMock.Verify(x => x.GetByIdAsync(teamMemberId), Times.Once());
        _teamMemberRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<TeamMember>()), Times.Never());
    }

    [Fact]
    public async Task DeleteAsync_WithValidRequest_ReturnsSuccess()
    {
        // Arrange
        const int teamMemberId = 123;
        var teamMember = _teamMemberTestDataMock.CreateTeamMember(teamMemberId);

        _teamMemberRepositoryMockSetup.GetTeamMemberById(teamMember);
        _teamMemberRepositoryMockSetup.TeamMemberDelete();

        // Act
        var result = await _teamMemberService.DeleteAsync(teamMemberId);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();

        _teamMemberRepositoryMock.Verify(x => x.GetByIdAsync(teamMemberId), Times.Once());
        _teamMemberRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<TeamMember>()), Times.Once());
    }

    [Fact]
    public async Task DeleteAsync_WhenTeamMemberNotFound_ReturnsResourceNotFoundError()
    {
        // Arrange
        const int teamMemberId = 123;

        _teamMemberRepositoryMockSetup.GetTeamMemberByIdNotFound(teamMemberId);

        // Act
        var result = await _teamMemberService.DeleteAsync(teamMemberId);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(nameof(ErrorType.ResourceNotFound));

        _teamMemberRepositoryMock.Verify(x => x.GetByIdAsync(teamMemberId), Times.Once());
        _teamMemberRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<TeamMember>()), Times.Never());
    }

    [Fact]
    public async Task GetAllAsync_WithValidRequest_ReturnsSuccess()
    {
        // Arrange
        var request = _teamMemberTestDataMock.CreatePageRequest();
        var teamMembers =
            _teamMemberTestDataMock.CreateTeamMembers(request.Page, request.PageSize, request.SortBy,
                request.Descending);

        _teamMemberRepositoryMockSetup.TeamMemberPageList(teamMembers, request);

        // Act
        var result = await _teamMemberService.GetAllAsync(request);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Items.ShouldNotBeNull();
        result.Value.Page.ShouldBe(request.Page);
        result.Value.PageSize.ShouldBe(request.PageSize);
        result.Value.SortBy.ShouldBe(request.SortBy);
        result.Value.Descending.ShouldBe(request.Descending);

        _teamMemberRepositoryMock.Verify(
            x => x.GetPageList(request.Page, request.PageSize, request.SortBy, request.Descending,
                CancellationToken.None), Times.Once());
    }

    [Fact]
    public async Task GetAllAsync_WhenTeamMemberIsEmpty_ReturnsEmptyList()
    {
        // Arrange
        var request = _teamMemberTestDataMock.CreatePageRequest();
        var teamMembers =
            _teamMemberTestDataMock.CreateTeamMembers(request.Page, request.PageSize, request.SortBy,
                request.Descending, 0);

        _teamMemberRepositoryMockSetup.TeamMemberPageList(teamMembers, request);

        // Act
        var result = await _teamMemberService.GetAllAsync(request);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Items.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_WithValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        const int teamMemberId = 123;
        var teamMember = _teamMemberTestDataMock.CreateTeamMember(teamMemberId);
        
        _teamMemberRepositoryMockSetup.GetTeamMemberById(teamMember);
        
        // Act
        var result = await _teamMemberService.GetByIdAsync(teamMemberId);
        
        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Id.ShouldBe(teamMemberId);
        
        _teamMemberRepositoryMock.Verify(x => x.GetByIdAsync(teamMemberId), Times.Once());
    }

    [Fact]
    public async Task GetByIdAsync_WhenTeamMemberIsNotFound_ReturnsResourceNotFoundError()
    {
        // Arrange
        const int teamMemberId = 123;
        _teamMemberRepositoryMockSetup.GetTeamMemberByIdNotFound(teamMemberId);
        
        // Act
        var result = await _teamMemberService.GetByIdAsync(teamMemberId);
        
        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(nameof(ErrorType.ResourceNotFound));
    }
}