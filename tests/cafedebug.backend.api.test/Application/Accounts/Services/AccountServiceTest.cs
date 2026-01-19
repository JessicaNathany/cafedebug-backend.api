using cafedebug.backend.api.test.Shared;
using cafedebug.backend.api.test.Shared.Mocks.Accounts;
using cafedebug.backend.api.test.Shared.Setups.Accounts;
using cafedebug.backend.api.test.Shared.Verifications.Account;
using cafedebug.backend.application.Accounts.Services;
using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Accounts.Services;
using cafedebug_backend.domain.Interfaces.Repositories;
using cafedebug_backend.domain.Messages.Email.Services;
using cafedebug_backend.domain.Shared.Errors;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace cafedebug.backend.api.test.Application.Accounts.Services;

[Collection("PodcastTests")]
public class AccountServiceTest : BaseTest
{
    private readonly AccountService _accountService;
    private readonly AccountTestDataMock _accountTestDataMock;
    private readonly UserRepositoryMockSetup _userRepositoryMockSetup;
    private readonly EmailServiceMockSetup _emailServiceMockSetup;
    private readonly UserRepositoryVerification _userVerifications;
    private readonly EmailServiceVerification _emailVerifications;
    private readonly Mock<IPasswordHasher<UserAdmin>> _passwordHasherMock;
    private readonly Mock<IJWTService> _jwtServiceMock;
    public AccountServiceTest()
    {
        _accountTestDataMock = new AccountTestDataMock(Fixture);

        var userRepository = new Mock<IUserRepository>();
        var emailService = new Mock<IEmailService>();
        _passwordHasherMock = new Mock<IPasswordHasher<UserAdmin>>();
        _jwtServiceMock = new Mock<IJWTService>();

        _accountService = new AccountService(emailService.Object, userRepository.Object, _passwordHasherMock.Object, _jwtServiceMock.Object);
        _emailServiceMockSetup = new EmailServiceMockSetup(emailService);
        _userRepositoryMockSetup = new UserRepositoryMockSetup(userRepository);
        _userVerifications = new UserRepositoryVerification(userRepository);
        _emailVerifications = new EmailServiceVerification(emailService);
    }

    [Fact]
    public async Task SendEmailForgotPassword_WithValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var request = _accountTestDataMock.SendEmailRequest();
        
        _emailServiceMockSetup.SendEmailSuccess();

        // Act
        var result = await _accountService.SendEmailForgotPassword(request);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        _emailVerifications.VerifyEmailSent(Times.Once());
    }

    [Fact]
    public async Task SendEmailForgotPassword_WhenEmailServiceThrows_PropagatesException()
    {
        // Arrange
        var request = _accountTestDataMock.SendEmailRequest();
        var expectedException = new InvalidOperationException("Email service down");

        _emailServiceMockSetup.SendEmailThrows(expectedException);

        // Act
        var act = async () => await _accountService.SendEmailForgotPassword(request);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Email service down");
        
        _emailVerifications.VerifyEmailSent(Times.Once());
    }

    [Fact]
    public async Task ResetPassword_WithValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var request = _accountTestDataMock.ChangePasswordRequest();
        var user = new UserAdmin 
        { 
            Id = 1, 
            Email = request.Email, 
            Name = "Test User",
            HashedPassword = "oldhashedpassword"
        };
        var newHashedPassword = "newhashedpassword";

        _userRepositoryMockSetup.GetUserByEmail(user);
        _passwordHasherMock.Setup(x => x.HashPassword(null, request.NewPassword))
            .Returns(newHashedPassword);

        // Act
        var result = await _accountService.ResetPassword(request);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        user.HashedPassword.Should().Be(newHashedPassword);
        user.Email.Should().Be(request.Email);
        user.LastUpdate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));

        _userVerifications.VerifyGetUserByEmail(request.Email, Times.Once());
        _passwordHasherMock.Verify(x => x.HashPassword(null, request.NewPassword), Times.Once());
    }

    [Fact]
    public async Task ResetPassword_WhenUserNotFound_ReturnsUserNotFoundError()
    {
        // Arrange
        var request = _accountTestDataMock.ChangePasswordRequest();
        
        _userRepositoryMockSetup.GetUserByEmailNotFound(request.Email);

        // Act
        var result = await _accountService.ResetPassword(request);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be(nameof(ErrorType.ResourceNotFound));

        _userVerifications.VerifyGetUserByEmail(request.Email, Times.Once());
        _passwordHasherMock.Verify(x => x.HashPassword(It.IsAny<UserAdmin>(), It.IsAny<string>()), Times.Never());
    }

    [Fact]
    public async Task ChangePassword_WithValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var request = _accountTestDataMock.ChangePasswordRequest();
        var user = new UserAdmin 
        { 
            Id = 1, 
            Email = request.Email, 
            Name = "Test User",
            HashedPassword = "oldhashedpassword"
        };
        var newHashedPassword = "newhashedpassword";

        _userRepositoryMockSetup.GetUserByEmail(user);
        _userRepositoryMockSetup.UserUpdateSuccess();
        _passwordHasherMock.Setup(x => x.HashPassword(null, request.NewPassword))
            .Returns(newHashedPassword);

        // Act
        var result = await _accountService.ChangePassword(request);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        // verify if password was changed
        user.HashedPassword.Should().Be(newHashedPassword);

        _userVerifications.VerifyGetUserByEmail(request.Email, Times.Once());
        _userVerifications.VerifyUserUpdated(Times.Once());
        _userVerifications.VerifyUserSaved(Times.Once());
        _passwordHasherMock.Verify(x => x.HashPassword(null, request.NewPassword), Times.Once());
    }

    [Fact]
    public async Task ChangePassword_WhenUserNotFound_ReturnsUserNotFoundError()
    {
        // Arrange
        var request = _accountTestDataMock.ChangePasswordRequest();
        
        _userRepositoryMockSetup.GetUserByEmailNotFound(request.Email);

        // Act
        var result = await _accountService.ChangePassword(request);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be(nameof(ErrorType.ResourceNotFound));

        _userVerifications.VerifyGetUserByEmail(request.Email, Times.Once());
        _userVerifications.VerifyUserUpdated(Times.Never());
        _userVerifications.VerifyUserSaved(Times.Never());
        _passwordHasherMock.Verify(x => x.HashPassword(It.IsAny<UserAdmin>(), It.IsAny<string>()), Times.Never());
    }

    [Fact]
    public async Task ChangePassword_WhenUpdateThrows_PropagatesException()
    {
        // Arrange
        var request = _accountTestDataMock.ChangePasswordRequest();
        var user = new UserAdmin 
        { 
            Id = 1, 
            Email = request.Email, 
            Name = "Test User",
            HashedPassword = "oldhashedpassword"
        };
        var expectedException = new InvalidOperationException("Database connection failed");

        _userRepositoryMockSetup.GetUserByEmail(user);
        _userRepositoryMockSetup.UserUpdateThrows(expectedException);
        _passwordHasherMock.Setup(x => x.HashPassword(null, request.NewPassword))
            .Returns("newhashedpassword");

        // Act
        var act = async () => await _accountService.ChangePassword(request);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Database connection failed");
        
        _userVerifications.VerifyGetUserByEmail(request.Email, Times.Once());
        _userVerifications.VerifyUserUpdated(Times.Once());
        _userVerifications.VerifyUserSaved(Times.Never()); 
    }

    [Fact]
    public async Task ForgotPassword_WithValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var request = _accountTestDataMock.ForgotPasswordRequest();
        var user = new UserAdmin 
        { 
            Id = 1, 
            Email = request.Email, 
            Name = request.Name,
            HashedPassword = "hashedpassword"
        };
        var resetToken = "reset-token-123";

        _userRepositoryMockSetup.GetUserByEmail(user);
        _jwtServiceMock.Setup(x => x.GenerateResetToken(user.Id))
            .Returns(resetToken);
        _emailServiceMockSetup.SendEmailSuccess();

        // Act
        var result = await _accountService.ForgotPassword(request);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        _userVerifications.VerifyGetUserByEmail(request.Email, Times.Once());
        _jwtServiceMock.Verify(x => x.GenerateResetToken(user.Id), Times.Once());
        _emailVerifications.VerifyEmailSent(Times.Once());
    }

    [Fact]
    public async Task ForgotPassword_WhenUserNotFound_ReturnsUserNotFoundError()
    {
        // Arrange
        var request = _accountTestDataMock.ForgotPasswordRequest();
        
        _userRepositoryMockSetup.GetUserByEmailNotFound(request.Email);

        // Act
        var result = await _accountService.ForgotPassword(request);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be(nameof(ErrorType.ResourceNotFound));

        _userVerifications.VerifyGetUserByEmail(request.Email, Times.Once());
        _jwtServiceMock.Verify(x => x.GenerateResetToken(It.IsAny<int>()), Times.Never());
        _emailVerifications.VerifyEmailSent(Times.Never());
    }

    [Fact]
    public async Task VerifyEmail_WhenCalled_ThrowsNotImplementedException()
    {
        // Arrange
        var email = "cafedebug@gmail.com";

        // Act
        var act = async () => await _accountService.VerifyEmail(email);

        // Assert
        await act.Should().ThrowAsync<NotImplementedException>();
    }
}

