using System.Linq.Expressions;
using System.Text.Json;
using AutoFixture;
using cafedebug.backend.api.test.Shared;
using cafedebug.backend.application.Accounts.DTOs.Requests;
using cafedebug.backend.application.Accounts.Services;
using cafedebug.backend.application.Common.Pagination;
using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Interfaces.Repositories;
using cafedebug_backend.domain.Shared.Errors;
using cafedebug_backend.infrastructure.Data.Pagination;
using Microsoft.AspNetCore.Identity;
using MockQueryable;
using Moq;
using Shouldly;
using Xunit;

namespace cafedebug.backend.api.test.Application.Accounts.Services;

[Collection("PodcastTests")]
public class UserServiceTest : BaseTest
{
    private readonly UserService _userService;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher<UserAdmin>> _passwordHasherMock;

    public UserServiceTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher<UserAdmin>>();
        _userService = new UserService(_userRepositoryMock.Object, _passwordHasherMock.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_ReturnsCreatedUserAndHidesPasswordFromJson()
    {
        // Arrange
        var request = new UserAdminRequest
        {
            Name = "Cafe Debug",
            Email = "admin@cafedebug.com",
            Password = "123456"
        };

        _userRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<UserAdmin, bool>>>()))
            .ReturnsAsync(false);

        _passwordHasherMock
            .Setup(x => x.HashPassword(null, request.Password))
            .Returns("hashed-password");

        _userRepositoryMock
            .Setup(x => x.SaveAsync(It.IsAny<UserAdmin>()))
            .Callback<UserAdmin>(user => user.Id = 10)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _userService.CreateAsync(request);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Id.ShouldBe(10);
        result.Value.Name.ShouldBe(request.Name);
        result.Value.Email.ShouldBe(request.Email);

        var responseJson = JsonSerializer.Serialize(result.Value);
        responseJson.ShouldNotContain("hashedPassword");
    }

    [Fact]
    public async Task CreateAsync_WhenEmailAlreadyExists_ReturnsExistingRegisterError()
    {
        // Arrange
        var request = new UserAdminRequest
        {
            Name = "Cafe Debug",
            Email = "admin@cafedebug.com",
            Password = "123456"
        };

        _userRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<UserAdmin, bool>>>()))
            .ReturnsAsync(true);

        // Act
        var result = await _userService.CreateAsync(request);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(nameof(ErrorType.ExistingRegister));
        _userRepositoryMock.Verify(x => x.SaveAsync(It.IsAny<UserAdmin>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenEmailIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new UserAdminRequest
        {
            Name = "Cafe Debug",
            Email = "admin-cafedebug.com",
            Password = "123456"
        };

        // Act
        var result = await _userService.CreateAsync(request);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(nameof(ErrorType.BadRequest));
    }

    [Fact]
    public async Task UpdateAsync_WhenUserNotFound_ReturnsResourceNotFound()
    {
        // Arrange
        const int id = 99;
        var request = new UserAdminRequest
        {
            Name = "Cafe Debug",
            Email = "admin@cafedebug.com",
            Password = "123456"
        };

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync((UserAdmin?)null);

        // Act
        var result = await _userService.UpdateAsync(request, id);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(nameof(ErrorType.ResourceNotFound));
    }

    [Fact]
    public async Task GetAllAsync_WithValidPageRequest_ReturnsPagedUsers()
    {
        // Arrange
        var request = new PageRequest
        {
            Page = 1,
            PageSize = 10,
            SortBy = "name",
            Descending = false
        };

        var users = Fixture.CreateMany<UserAdmin>(3).ToList();
        var usersQuery = users.BuildMock();
        var pagedUsers = new PagedList<UserAdmin>(usersQuery, request.Page, request.PageSize, request.SortBy, request.Descending);

        _userRepositoryMock
            .Setup(x => x.GetPageList(request.Page, request.PageSize, request.SortBy, request.Descending, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedUsers);

        // Act
        var result = await _userService.GetAllAsync(request);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Items.Count.ShouldBe(3);
        result.Value.Page.ShouldBe(request.Page);
        result.Value.PageSize.ShouldBe(request.PageSize);
    }
}
