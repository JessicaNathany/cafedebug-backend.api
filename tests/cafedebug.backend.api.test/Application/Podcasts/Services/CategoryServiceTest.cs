using cafedebug.backend.api.test.Shared;
using cafedebug.backend.api.test.Shared.Mocks.Podcasts;
using cafedebug.backend.api.test.Shared.Setups.Podcasts;
using cafedebug.backend.api.test.Shared.Verifications.Podcasts;
using cafedebug.backend.application.Podcasts.Services;
using cafedebug_backend.domain.Podcasts;
using cafedebug_backend.domain.Podcasts.Repositories;
using cafedebug_backend.domain.Shared.Errors;
using FluentAssertions;
using Moq;
using Xunit;

namespace cafedebug.backend.api.test.Application.Podcasts.Services
{
    [Collection("PodcastTests")]
    public class CategoryServiceTest : BaseTest
    {
        private readonly CategoryService _categoryService;

        private readonly CategoryTestDataMock _categoryTestDataMock;
        private readonly CategoryRepositoryMockSetup _categoryRepositoryMockSetup;
        private readonly CategoryRepositoryVerifications _categoryVerifications;

        public CategoryServiceTest()
        {
            _categoryTestDataMock = new CategoryTestDataMock(Fixture);

            var categoryRepository = new Mock<ICategoryRepository>();

            _categoryService = new CategoryService(categoryRepository.Object);
            _categoryRepositoryMockSetup = new CategoryRepositoryMockSetup(categoryRepository);
            _categoryVerifications = new CategoryRepositoryVerifications(categoryRepository);
        }

        [Fact]
        public async Task CreateAsync_WithValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = _categoryTestDataMock.CreateCategoryRequest();

            _categoryRepositoryMockSetup.CategoryDoesNotExist();


            Category? savedCategory = null;
            _categoryRepositoryMockSetup.CategorySave(e => savedCategory = e);

            // Act
            var result = await _categoryService.CreateAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();

            var response = result.Value;
            response.Name.Should().Be(request.Name);

            savedCategory.Should().NotBeNull();

            _categoryVerifications.VerifyCategorySaved(Times.Once());
        }

        [Fact]
        public async Task CreateAsync_WhenSaveThrowsException_PropagatesException()
        {
            // Arrange
            var request = _categoryTestDataMock.CreateCategoryRequest();
            var expectedException = new InvalidOperationException("DB down");

            _categoryRepositoryMockSetup.CategoryDoesNotExist();
            _categoryRepositoryMockSetup.CategorySaveThrows(expectedException);

            // Act
            var act = async () => await _categoryService.CreateAsync(request);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("DB down");

            _categoryVerifications.VerifyCategorySaved(Times.Once());
        }

        [Fact]
        public async Task CreateAsync_WhenCategoryTitleAlreadyExists_ReturnsExistingRegisterError()
        {
            // Arrange
            var request = _categoryTestDataMock.CreateCategoryRequest();
            _categoryRepositoryMockSetup.CategoryExists();

            // Act
            var result = await _categoryService.CreateAsync(request);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(nameof(ErrorType.ExistingRegister));

            _categoryVerifications.VerifyCategoryExistenceChecked(Times.Once());
            _categoryVerifications.VerifyCategoryRetrieved(It.IsAny<int>(), Times.Never());
            _categoryVerifications.VerifyCategorySaved(Times.Never());
        }

        [Fact]
        public async Task UpdateAsync_WithValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            const int categoryId = 15;
            var banner = _categoryTestDataMock.CreateCategory(categoryId);
            var request = _categoryTestDataMock.CreateCategoryRequest();

            _categoryRepositoryMockSetup.GetCategoryById(banner);


            _categoryRepositoryMockSetup.CategoryUpdate();

            // Act
            var result = await _categoryService.UpdateAsync(request, categoryId);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();

            var response = result.Value;
            response.Name.Should().Be(request.Name);


            _categoryVerifications.VerifyCategoryRetrieved(categoryId, Times.Once());
            _categoryVerifications.VerifyCategoryUpdated(Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_WhenCategoryNotFound_ReturnsResourceNotFoundError()
        {
            // Arrange
            const int categoryId = 15;
            var request = _categoryTestDataMock.CreateCategoryRequest();

            _categoryRepositoryMockSetup.GetCategoryByIdNotFound(categoryId);

            // Act
            var result = await _categoryService.UpdateAsync(request, categoryId);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Code.Should().Be(nameof(ErrorType.ResourceNotFound));
        }

        [Fact]
        public async Task DeleteAsync_WithValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            const int categoryId = 223;
            var category = _categoryTestDataMock.CreateCategory(categoryId);

            _categoryRepositoryMockSetup.GetCategoryById(category);

            // Act
            var result = await _categoryService.DeleteAsync(categoryId);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();

            _categoryVerifications.VerifyCategoryRetrieved(categoryId, Times.Once());
            _categoryVerifications.VerifyCategoryDeleted(category, Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_WhenCategoryNotFound_ReturnsResourceNotFoundError()
        {
            // Arrange
            const int categoryId = 9999;

            _categoryRepositoryMockSetup.GetCategoryByIdNotFound(categoryId);

            // Act
            var result = await _categoryService.DeleteAsync(categoryId);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Code.Should().Be(nameof(ErrorType.ResourceNotFound));
        }

        [Fact]
        public async Task GetAllAsync_WithValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var request = _categoryTestDataMock.CreatePageRequest();
            var pagedResult = _categoryTestDataMock.CreateCategoryPagedResult(request.Page, request.PageSize, request.SortBy, request.Descending);

            _categoryRepositoryMockSetup.CategoryGetPageList(pagedResult, request);

            // Act
            var result = await _categoryService.GetAllAsync(request);

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

            _categoryVerifications.VerifyCategoryPageListRetrieved(request.Page, request.PageSize, request.SortBy, request.Descending, Times.Once());
        }

        [Fact]
        public async Task GetAllAsync_WhenCategoryIsEmpty_ReturnsSuccessResult()
        {
            // Arrange
            var request = _categoryTestDataMock.CreatePageRequest();
            var pagedResult = _categoryTestDataMock.CreateCategoryPagedResult(request.Page, request.PageSize, request.SortBy, request.Descending, 0);

            _categoryRepositoryMockSetup.CategoryGetPageList(pagedResult, request);

            // Act
            var result = await _categoryService.GetAllAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Items.Should().BeEmpty();
            result.Value.Page.Should().Be(request.Page);
            result.Value.PageSize.Should().Be(request.PageSize);
            result.Value.SortBy.Should().Be(request.SortBy);
            result.Value.Descending.Should().Be(request.Descending);

            _categoryVerifications.VerifyCategoryPageListRetrieved(request.Page, request.PageSize, request.SortBy, request.Descending, Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_WithValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            const int categoryId = 15;
            var category = _categoryTestDataMock.CreateCategory(categoryId);

            _categoryRepositoryMockSetup.GetCategoryById(category);

            // Act
            var result = await _categoryService.GetByIdAsync(categoryId);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();

            _categoryVerifications.VerifyCategoryRetrieved(categoryId, Times.Once());
        }
    }
}
