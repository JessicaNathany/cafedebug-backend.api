using cafedebug.backend.api.test.Shared;
using cafedebug.backend.api.test.Shared.Mocks.Banners;
using cafedebug.backend.api.test.Shared.Mocks.Podcasts;
using cafedebug.backend.api.test.Shared.Setups;
using cafedebug.backend.api.test.Shared.Setups.Podcasts;
using cafedebug.backend.api.test.Shared.Verifications;
using cafedebug.backend.api.test.Shared.Verifications.Podcasts;
using cafedebug.backend.application.Banners.Services;
using cafedebug_backend.domain.Banners;
using cafedebug_backend.domain.Banners.Repositories;
using cafedebug_backend.domain.Shared.Errors;
using FluentAssertions;
using Moq;
using Xunit;

namespace cafedebug.backend.api.test.Application.Banners.Services;

[Collection("PodcastTests")]
public class BannerServiceTest : BaseTest
{
    private readonly BannerService _bannerService;
    private readonly BannerTestDataMock _bannerTestDataMock;
    private readonly BannerRepositoryMockSetup _bannerRepositoryMockSetup;
    private readonly BannerRepositoryVerification _bannerVerifications;

    public BannerServiceTest()
    {
        _bannerTestDataMock = new BannerTestDataMock(Fixture);

        var bannerRepository = new Mock<IBannerRepository>();

        _bannerService = new BannerService(bannerRepository.Object);
        _bannerRepositoryMockSetup = new BannerRepositoryMockSetup(bannerRepository);
        _bannerVerifications = new BannerRepositoryVerification(bannerRepository);
    }
    [Fact]
    public async Task CreateAsync_WithValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var request = _bannerTestDataMock.CreateBannerRequest();

        _bannerRepositoryMockSetup.BannerDoesNotExist();
        

        Banner? savedBanner = null;
        _bannerRepositoryMockSetup.BannerSave(e => savedBanner = e);

        // Act
        var result = await _bannerService.CreateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        var response = result.Value;
        response.Name.Should().Be(request.Name);

        savedBanner.Should().NotBeNull();

        _bannerVerifications.VerifyBannerSaved(Times.Once());
    }

    [Fact]
    public async Task CreateAsync_WhenSaveThrowsException_PropagatesException()
    {
        // Arrange
        var request = _bannerTestDataMock.CreateBannerRequest();
        var expectedException = new InvalidOperationException("DB down");

        _bannerRepositoryMockSetup.BannerDoesNotExist();
        _bannerRepositoryMockSetup.BannerSaveThrows(expectedException);

        // Act
        var act = async () => await _bannerService.CreateAsync(request);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("DB down");
        
        _bannerVerifications.VerifyBannerSaved(Times.Once());
    }

    [Fact]
    public async Task CreateAsync_WhenBannerTitleAlreadyExists_ReturnsExistingRegisterError()
    {
        // Arrange
        var request = _bannerTestDataMock.CreateBannerRequest();
        _bannerRepositoryMockSetup.BannerExists();

        // Act
        var result = await _bannerService.CreateAsync(request);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(nameof(ErrorType.ExistingRegister));

        _bannerVerifications.VerifyBannerExistenceChecked(Times.Once());
        _bannerVerifications.VerifyBannerRetrieved(It.IsAny<int>(), Times.Never());
        _bannerVerifications.VerifyBannerSaved(Times.Never());
    }

    [Fact]
    public async Task UpdateAsync_WithValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        const int bannerId = 15;
        var banner = _bannerTestDataMock.CreateBanner(bannerId);
        var request = _bannerTestDataMock.CreateBannerRequest();

        _bannerRepositoryMockSetup.GetBannerById(banner);


        _bannerRepositoryMockSetup.BannerUpdate();

        // Act
        var result = await _bannerService.UpdateAsync(request, bannerId);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        var response = result.Value;
        response.Name.Should().Be(request.Name);


        _bannerVerifications.VerifyBannerRetrieved(bannerId, Times.Once());
        _bannerVerifications.VerifyBannerUpdated(Times.Once());
    }

    [Fact]
    public async Task UpdateAsync_WhenBannerNotFound_ReturnsResourceNotFoundError()
    {
        // Arrange
        const int bannerId = 15;
        var request = _bannerTestDataMock.CreateBannerRequest();

        _bannerRepositoryMockSetup.GetBannerByIdNotFound(bannerId);

        // Act
        var result = await _bannerService.UpdateAsync(request, bannerId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(nameof(ErrorType.ResourceNotFound));
    }

    [Fact]
    public async Task DeleteAsync_WithValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        const int bannerId = 223;
        var banner = _bannerTestDataMock.CreateBanner(bannerId);

        _bannerRepositoryMockSetup.GetBannerById(banner);

        // Act
        var result = await _bannerService.DeleteAsync(bannerId);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        _bannerVerifications.VerifyBannerRetrieved(bannerId, Times.Once());
        _bannerVerifications.VerifyBannerDeleted(banner, Times.Once());
    }

    [Fact]
    public async Task DeleteAsync_WhenBannerNotFound_ReturnsResourceNotFoundError()
    {
        // Arrange
        const int bannerId = 9999;

        _bannerRepositoryMockSetup.GetBannerByIdNotFound(bannerId);

        // Act
        var result = await _bannerService.DeleteAsync(bannerId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(nameof(ErrorType.ResourceNotFound));
    }

    [Fact]
    public async Task GetAllAsync_WithValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var request = _bannerTestDataMock.CreatePageRequest();
        var pagedResult = _bannerTestDataMock.CreateBannerPagedResult(request.Page, request.PageSize, request.SortBy, request.Descending);

        _bannerRepositoryMockSetup.BannerGetPageList(pagedResult, request);

        // Act
        var result = await _bannerService.GetAllAsync(request);

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

        _bannerVerifications.VerifyBannerPageListRetrieved(request.Page, request.PageSize, request.SortBy, request.Descending, Times.Once());
    }

    [Fact]
    public async Task GetAllAsync_WhenBannerIsEmpty_ReturnsSuccessResult()
    {
        // Arrange
        var request = _bannerTestDataMock.CreatePageRequest();
        var pagedResult = _bannerTestDataMock.CreateBannerPagedResult(request.Page, request.PageSize, request.SortBy, request.Descending, 0);

        _bannerRepositoryMockSetup.BannerGetPageList(pagedResult, request);

        // Act
        var result = await _bannerService.GetAllAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Items.Should().BeEmpty();
        result.Value.Page.Should().Be(request.Page);
        result.Value.PageSize.Should().Be(request.PageSize);
        result.Value.SortBy.Should().Be(request.SortBy);
        result.Value.Descending.Should().Be(request.Descending);

        _bannerVerifications.VerifyBannerPageListRetrieved(request.Page, request.PageSize, request.SortBy, request.Descending, Times.Once());
    }

    [Fact]
    public async Task GetByIdAsync_WithValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        const int bannerId = 15;
        var banner = _bannerTestDataMock.CreateBanner(bannerId);

        _bannerRepositoryMockSetup.GetBannerById(banner);

        // Act
        var result = await _bannerService.GetByIdAsync(bannerId);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();

        _bannerVerifications.VerifyBannerRetrieved(bannerId, Times.Once());
    }

}