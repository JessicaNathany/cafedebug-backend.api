using cafedebug_backend.domain.Episodes.Repositories;
using cafedebug_backend.domain.Errors;
using cafedebug_backend.domain.Podcasts;
using cafedebug_backend.domain.Podcasts.Repositories;
using cafedebug_backend.domain.Shared.Errors;
using cafedebug.backend.api.test.Shared;
using cafedebug.backend.api.test.Shared.Mocks.Podcasts;
using cafedebug.backend.api.test.Shared.Setups.Podcasts;
using cafedebug.backend.api.test.Shared.Verifications.Podcasts;
using cafedebug.backend.application.Podcasts.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace cafedebug.backend.api.test.Application.Podcasts.Services;

[Collection("PodcastTests")]
public class EpisodeServiceTest : BaseTest
{
    private readonly EpisodeService _episodeService;
    
    private readonly EpisodeTestDataMock _episodeTestDataMock;
    private readonly CategoryTestDataMock _categoryTestDataMock;
    private readonly EpisodeRepositoryMockSetup _episodeRepositoryMockSetup;
    private readonly CategoryRepositoryMockSetup _categoryRepositoryMockSetup;
    private readonly EpisodeRepositoryVerifications _episodeRepositoryVerifications;
    private readonly CategoryRepositoryVerifications _categoryRepositoryVerifications;

    public EpisodeServiceTest()
    {
        _episodeTestDataMock = new EpisodeTestDataMock(Fixture);
        _categoryTestDataMock = new CategoryTestDataMock(Fixture);
        
        var episodeRepository = new Mock<IEpisodeRepository>();
        var categoryRepository = new Mock<ICategoryRepository>();
        
        _episodeService = new EpisodeService(episodeRepository.Object, categoryRepository.Object, Mapper);
        _categoryRepositoryVerifications = new CategoryRepositoryVerifications(categoryRepository);
        _episodeRepositoryVerifications = new EpisodeRepositoryVerifications(episodeRepository);
        _episodeRepositoryMockSetup = new EpisodeRepositoryMockSetup(episodeRepository);
        _categoryRepositoryMockSetup = new CategoryRepositoryMockSetup(categoryRepository);
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var category = _categoryTestDataMock.CreateCategory(1001);
        var request = _episodeTestDataMock.CreateEpisodeRequest(category.Id);

        _episodeRepositoryMockSetup.EpisodeDoesNotExist();
        _categoryRepositoryMockSetup.CategoryExists(category);

        Episode? savedEpisode = null;
        _episodeRepositoryMockSetup.EpisodeSave(e => savedEpisode = e);

        // Act
        var result = await _episodeService.CreateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        var response = result.Value;
        response.Title.Should().Be(request.Title);
        response.Category.Id.Should().Be(category.Id);

        savedEpisode.Should().NotBeNull();

        _categoryRepositoryVerifications.VerifyCategoryRetrieved(category.Id, Times.Once());
        _episodeRepositoryVerifications.VerifyEpisodeSaved(Times.Once());
    }

    [Fact]
    public async Task CreateAsync_WhenCategoryDoesNotExist_ReturnsResourceNotFoundError()
    {
        // Arrange
        var request = _episodeTestDataMock.CreateEpisodeRequest(categoryId: 9999);

        _episodeRepositoryMockSetup.EpisodeDoesNotExist();
        _categoryRepositoryMockSetup.CategoryDoesNotExist(request.CategoryId);

        // Act
        var result = await _episodeService.CreateAsync(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(nameof(ErrorType.ResourceNotFound));

        _categoryRepositoryVerifications.VerifyCategoryRetrieved(request.CategoryId, Times.Once());
        _episodeRepositoryVerifications.VerifyEpisodeSaved(Times.Never());
    }

    [Fact]
    public async Task CreateAsync_WhenSaveThrowsException_PropagatesException()
    {
        // Arrange
        var category = _categoryTestDataMock.CreateCategory(id: 2002);
        var request = _episodeTestDataMock.CreateEpisodeRequest(categoryId: category.Id);
        var expectedException = new InvalidOperationException("DB down");

        _episodeRepositoryMockSetup.EpisodeDoesNotExist();
        _categoryRepositoryMockSetup.CategoryExists(category);
        _episodeRepositoryMockSetup.EpisodeSaveThrows(expectedException);

        // Act
        var act = async () => await _episodeService.CreateAsync(request);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("DB down");

        _categoryRepositoryVerifications.VerifyCategoryRetrieved(category.Id, Times.Once());
        _episodeRepositoryVerifications.VerifyEpisodeSaved(Times.Once());
    }

    [Fact]
    public async Task CreateAsync_WhenEpisodeTitleAlreadyExists_ReturnsExistingRegisterError()
    {
        // Arrange
        var request = _episodeTestDataMock.CreateEpisodeRequest();
        _episodeRepositoryMockSetup.EpisodeExists();

        // Act
        var result = await _episodeService.CreateAsync(request);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(nameof(ErrorType.ExistingRegister));

        _episodeRepositoryVerifications.VerifyEpisodeExistenceChecked(Times.Once());
        _categoryRepositoryVerifications.VerifyCategoryRetrieved(It.IsAny<int>(), Times.Never());
        _episodeRepositoryVerifications.VerifyEpisodeSaved(Times.Never());
    }
    
    [Fact]
    public async Task UpdateAsync_WithValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        const int episodeId = 223;
        var episode = _episodeTestDataMock.CreateEpisode(episodeId);
        var category = _categoryTestDataMock.CreateCategory(1001);
        var request = _episodeTestDataMock.CreateEpisodeRequest(category.Id);

        _episodeRepositoryMockSetup.GetEpisodeById(episode);
        _categoryRepositoryMockSetup.CategoryExists(category);
        
        _episodeRepositoryMockSetup.EpisodeUpdate();

        // Act
        var result = await _episodeService.UpdateAsync(episodeId, request);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        var response = result.Value;
        response.Title.Should().Be(request.Title);
        response.Category.Id.Should().Be(category.Id);

        _episodeRepositoryVerifications.VerifyEpisodeRetrieved(episodeId, Times.Once());
        _categoryRepositoryVerifications.VerifyCategoryRetrieved(category.Id, Times.Once());
        _episodeRepositoryVerifications.VerifyEpisodeUpdated(Times.Once());
    }

    [Fact]
    public async Task UpdateAsync_WhenEpisodeNotFound_ReturnsResourceNotFoundError()
    {
        // Arrange
        const int episodeId = 9999;
        var request = _episodeTestDataMock.CreateEpisodeRequest();
        
        _episodeRepositoryMockSetup.GetEpisodeByIdNotFound(episodeId);
        
        // Act
        var result = await _episodeService.UpdateAsync(episodeId, request);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(nameof(ErrorType.ResourceNotFound));
    }

    [Fact]
    public async Task UpdateAsync_WhenCategoryNotFound_ReturnsResourceNotFoundError()
    {
        // Arrange
        const int episodeId = 223;
        const int categoryId = 9999;
        var episode = _episodeTestDataMock.CreateEpisode(episodeId);
        var request = _episodeTestDataMock.CreateEpisodeRequest(categoryId);

        _episodeRepositoryMockSetup.GetEpisodeById(episode);
        _categoryRepositoryMockSetup.CategoryDoesNotExist(categoryId);
        
        // Act
        var result = await _episodeService.UpdateAsync(episodeId, request);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(nameof(ErrorType.ResourceNotFound));
        
        _episodeRepositoryVerifications.VerifyEpisodeRetrieved(episodeId, Times.Once());
        _categoryRepositoryVerifications.VerifyCategoryRetrieved(categoryId, Times.Once());
        _episodeRepositoryVerifications.VerifyEpisodeUpdated(Times.Never());
    }

    [Fact]
    public async Task DeleteAsync_WithValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        const int episodeId = 223;
        var episode = _episodeTestDataMock.CreateEpisode(episodeId);
        
        _episodeRepositoryMockSetup.GetEpisodeById(episode);
        
        // Act
        var result = await _episodeService.DeleteAsync(episodeId);
        
        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        
        _episodeRepositoryVerifications.VerifyEpisodeRetrieved(episodeId, Times.Once());
        _episodeRepositoryVerifications.VerifyEpisodeDeleted(episode, Times.Once());
    }

    [Fact]
    public async Task DeleteAsync_WhenEpisodeNotFound_ReturnsResourceNotFoundError()
    {
        // Arrange
        const int episodeId = 9999;
        
        _episodeRepositoryMockSetup.GetEpisodeByIdNotFound(episodeId);
        
        // Act
        var result = await _episodeService.DeleteAsync(episodeId);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(nameof(ErrorType.ResourceNotFound));
    }

    [Fact]
    public async Task GetAllAsync_WithValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var request = _episodeTestDataMock.CreatePageRequest();
        var pagedResult = _episodeTestDataMock.CreateEpisodePagedResult(request.Page, request.PageSize, request.SortBy, request.Descending);

        _episodeRepositoryMockSetup.EpisodeGetPageList(pagedResult, request);
        
        // Act
        var result = await _episodeService.GetAllAsync(request);
        
        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Items.Should().NotBeNullOrEmpty();
        result.Value.Items.Should().HaveCount(pagedResult.TotalCount);
        result.Value.Page.Should().Be(request.Page);
        result.Value.PageSize.Should().Be(request.PageSize);
        result.Value.SortBy.Should().Be(request.SortBy);
        result.Value.Descending.Should().Be(request.Descending);
        
        _episodeRepositoryVerifications.VerifyEpisodePageListRetrieved(request.Page, request.PageSize, request.SortBy, request.Descending, Times.Once());
    }

    [Fact]
    public async Task GetAllAsync_WhenEpisodeIsEmpty_ReturnsSuccessResult()
    {
        // Arrange
        var request = _episodeTestDataMock.CreatePageRequest();
        var pagedResult = _episodeTestDataMock.CreateEpisodePagedResult(request.Page, request.PageSize, request.SortBy, request.Descending, 0);
        
        _episodeRepositoryMockSetup.EpisodeGetPageList(pagedResult, request);
        
        // Act
        var result = await _episodeService.GetAllAsync(request);
        
        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Items.Should().BeEmpty();
        result.Value.Page.Should().Be(request.Page);
        result.Value.PageSize.Should().Be(request.PageSize);
        result.Value.SortBy.Should().Be(request.SortBy);
        result.Value.Descending.Should().Be(request.Descending);
        
        _episodeRepositoryVerifications.VerifyEpisodePageListRetrieved(request.Page, request.PageSize, request.SortBy, request.Descending, Times.Once());
    }

    [Fact]
    public async Task GetByIdAsync_WithValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        const int episodeId = 223;
        var episode = _episodeTestDataMock.CreateEpisode(episodeId);
        
        _episodeRepositoryMockSetup.GetEpisodeById(episode);
        
        // Act
        var result = await _episodeService.GetByIdAsync(episodeId);
        
        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        
        _episodeRepositoryVerifications.VerifyEpisodeRetrieved(episodeId, Times.Once());
    }

    [Fact]
    public async Task GetByIdAsync_WhenEpisodeNotFound_ReturnsResourceNotFoundError()
    {
        // Arrange
        const int episodeId = 9999;
        
        _episodeRepositoryMockSetup.GetEpisodeByIdNotFound(episodeId);
        
        // Act
        var result = await _episodeService.GetByIdAsync(episodeId);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(nameof(ErrorType.ResourceNotFound));
        
        _episodeRepositoryVerifications.VerifyEpisodeRetrieved(episodeId, Times.Once());
    }
}