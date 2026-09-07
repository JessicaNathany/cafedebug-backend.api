using cafedebug.backend.api.test.Shared;
using cafedebug.backend.api.test.Shared.Setups.Accounts;
using cafedebug.backend.api.test.Shared.Verifications.Account;
using cafedebug.backend.application.Accounts.Services;
using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Accounts.Repositories;
using cafedebug_backend.domain.Accounts.Tokens;
using cafedebug_backend.domain.Interfaces.Repositories;
using cafedebug_backend.domain.Shared.Errors;
using cafedebug_backend.infrastructure.Security;
using Microsoft.Extensions.Time.Testing;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Shouldly;
using System.Text;
using Xunit;

namespace cafedebug.backend.api.test.Application.Accounts.Services;

[Collection("PodcastTests")]
public class JWTServiceTest : BaseTest
{
    private readonly JWTService _jwtService;
    private readonly UserRepositoryMockSetup _userRepositoryMockSetup;
    private readonly UserRepositoryVerification _userVerifications;
    private readonly Mock<IRefreshTokensRepository> _refreshTokensRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly JwtSettings _jwtSettings;
    private readonly FakeTimeProvider _timeProvider;

    public JWTServiceTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _refreshTokensRepositoryMock = new Mock<IRefreshTokensRepository>();
        _timeProvider = new FakeTimeProvider(new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero));

        // Configure JwtSettings
        _jwtSettings = new JwtSettings
        {
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ValidForMinutes = 15,
            RefreshTokenValidForMinutes = 240,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsASecretKeyForTestingPurposes123456")),
                SecurityAlgorithms.HmacSha256)
        };

        _jwtService = new JWTService(
            _jwtSettings,
            _refreshTokensRepositoryMock.Object,
            _userRepositoryMock.Object,
            _timeProvider);
        _userRepositoryMockSetup = new UserRepositoryMockSetup(_userRepositoryMock);
        _userVerifications = new UserRepositoryVerification(_userRepositoryMock);
    }

    [Fact]
    public async Task GenerateToken_WithValidCredentials_ReturnsSuccessResult()
    {
        // Arrange
        var email = "test@cafedebug.com";
        var password = "password123";
        var hashedPassword = GenerateSHA256(password); 
        var user = new UserAdmin
        {
            Id = 1,
            Email = email,
            Name = "Test User",
            HashedPassword = hashedPassword 
        };

        _userRepositoryMockSetup.GetUserByEmail(user);

        _refreshTokensRepositoryMock.Setup(x => x.GetByTokenByUserIdAsync(user.Id))
            .ReturnsAsync((RefreshTokens)null);
        
        _refreshTokensRepositoryMock.Setup(x => x.SaveAsync(It.IsAny<RefreshTokens>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _jwtService.GenerateToken(email, password);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.AccessToken.ShouldNotBeNullOrEmpty();
        result.Value.RefreshToken.ShouldNotBeNull();
        result.Value.TokenType.ShouldBe("Bearer");
        result.Value.ExpiresIn.ShouldBeGreaterThan(0);
        result.Value.RefreshToken.ExpirationDate.ShouldBe(
            _timeProvider.GetUtcNow().UtcDateTime.AddMinutes(_jwtSettings.RefreshTokenValidForMinutes));

        _userVerifications.VerifyGetUserByEmail(email, Times.Once());
    }

    [Fact]
    public async Task GenerateToken_WithEmptyEmail_ReturnsFailureResult()
    {
        // Arrange
        var email = "";
        var password = "password123";

        // Act
        var result = await _jwtService.GenerateToken(email, password);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldNotBeNull();

        _userVerifications.VerifyGetUserByEmail(Times.Never());
    }

    [Fact]
    public async Task GenerateToken_WithEmptyPassword_ReturnsFailureResult()
    {
        // Arrange
        var email = "test@cafedebug.com";
        var password = "";

        // Act
        var result = await _jwtService.GenerateToken(email, password);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldNotBeNull();

        _userVerifications.VerifyGetUserByEmail(Times.Never());
    }

    [Fact]
    public async Task GenerateToken_WithUserNotFound_ReturnsFailureResult()
    {
        // Arrange
        var email = "notfound@cafedebug.com";
        var password = "password123";

        _userRepositoryMockSetup.GetUserByEmailNotFound(email);

        // Act
        var result = await _jwtService.GenerateToken(email, password);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldNotBeNull();
        result.Error.Code.ShouldBe(nameof(ErrorType.ResourceNotFound));

        _userVerifications.VerifyGetUserByEmail(email, Times.Once());
    }

    [Fact]
    public async Task GenerateToken_WithInvalidPassword_ReturnsFailureResult()
    {
        // Arrange
        var email = "test@cafedebug.com";
        var correctPassword = "password123";
        var wrongPassword = "wrongpassword";
        var correctHash = GenerateSHA256(correctPassword); 
        
        var user = new UserAdmin
        {
            Id = 1,
            Email = email,
            Name = "Test User",
            HashedPassword = correctHash 
        };

        _userRepositoryMockSetup.GetUserByEmail(user);

        // Act
        var result = await _jwtService.GenerateToken(email, wrongPassword);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldNotBeNull();

        _userVerifications.VerifyGetUserByEmail(email, Times.Once());
        
    }

    private string GenerateSHA256(string password)
    {
        using (var sha256Hash = System.Security.Cryptography.SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            var builder = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
                builder.Append(bytes[i].ToString("x2"));

            return builder.ToString();
        }
    }

    [Fact]
    public async Task RefreshTokenAsync_WithValidToken_ReturnsSuccessResult()
    {
        // Arrange
        var refreshTokenString = "valid-refresh-token";
        var user = new UserAdmin
        {
            Id = 1,
            Email = "test@cafedebug.com",
            Name = "Test User",
            HashedPassword = "hashedpassword"
        };
        var now = _timeProvider.GetUtcNow().UtcDateTime;
        var refreshToken = RefreshTokens.Create(user.Id, user.Name, refreshTokenString, now.AddDays(1), now, now);

        _refreshTokensRepositoryMock.Setup(x => x.GetByTokenAsync(refreshTokenString))
            .ReturnsAsync(refreshToken);
        
        _refreshTokensRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<RefreshTokens>()))
            .Returns(Task.CompletedTask);
        
        _userRepositoryMock.Setup(x => x.GetByIdAsync(user.Id)).ReturnsAsync(user);

        // Act
        var result = await _jwtService.RefreshTokenAsync(refreshTokenString);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.AccessToken.ShouldNotBeNullOrEmpty();
        result.Value.RefreshToken.ShouldNotBeNull();
        result.Value.TokenType.ShouldBe("Bearer");
        result.Value.ExpiresIn.ShouldBeGreaterThan(0);

        _refreshTokensRepositoryMock.Verify(x => x.GetByTokenAsync(refreshTokenString), Times.Once());
        _refreshTokensRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<RefreshTokens>()), Times.Once());
        _userRepositoryMock.Verify(x => x.GetByIdAsync(user.Id), Times.Once());
    }

    [Fact]
    public async Task RefreshTokenAsync_WithExpiredToken_ReturnsFailureResult()
    {
        // Arrange
        var refreshTokenString = "expired-refresh-token";
        var now = _timeProvider.GetUtcNow().UtcDateTime;
        var expiredRefreshToken = RefreshTokens.Create(1, "Test User", refreshTokenString, now.AddDays(-1), now, now);

        _refreshTokensRepositoryMock.Setup(x => x.GetByTokenAsync(refreshTokenString))
            .ReturnsAsync(expiredRefreshToken);

        // Act
        var result = await _jwtService.RefreshTokenAsync(refreshTokenString);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldNotBeNull();

        _refreshTokensRepositoryMock.Verify(x => x.GetByTokenAsync(refreshTokenString), Times.Once());
        _refreshTokensRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<RefreshTokens>()), Times.Never());
    }

    [Fact]
    public async Task RefreshTokenAsync_WithInvalidToken_ReturnsFailureResult()
    {
        // Arrange
        var refreshTokenString = "invalid-refresh-token";

        _refreshTokensRepositoryMock.Setup(x => x.GetByTokenAsync(refreshTokenString))
            .ReturnsAsync((RefreshTokens)null);

        // Act
        var result = await _jwtService.RefreshTokenAsync(refreshTokenString);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldNotBeNull();

        _refreshTokensRepositoryMock.Verify(x => x.GetByTokenAsync(refreshTokenString), Times.Once());
        _refreshTokensRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<RefreshTokens>()), Times.Never());
    }

    [Fact]
    public void GenerateResetToken_WithValidUserId_ReturnsTokenString()
    {
        // Arrange
        var userId = 123;

        // Act
        var result = _jwtService.GenerateResetToken(userId);

        // Assert
        result.ShouldNotBeNullOrEmpty();
        result.ShouldContain(".");
    }

    [Fact]
    public async Task GetByTokenAsync_WithValidToken_ReturnsSuccessResult()
    {
        // Arrange
        var tokenString = "valid-token";
        var now = _timeProvider.GetUtcNow().UtcDateTime;
        var refreshToken = RefreshTokens.Create(1, "Test User", tokenString, now.AddDays(1), now, now);

        _refreshTokensRepositoryMock.Setup(x => x.GetByTokenAsync(tokenString))
            .ReturnsAsync(refreshToken);

        // Act
        var result = await _jwtService.GetByTokenAsync(tokenString);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Token.ShouldBe(tokenString);

        _refreshTokensRepositoryMock.Verify(x => x.GetByTokenAsync(tokenString), Times.Once());
    }

    [Fact]
    public async Task GetByTokenAsync_WithInvalidToken_ReturnsFailureResult()
    {
        // Arrange
        var tokenString = "invalid-token";

        _refreshTokensRepositoryMock.Setup(x => x.GetByTokenAsync(tokenString))
            .ReturnsAsync((RefreshTokens)null);

        // Act
        var result = await _jwtService.GetByTokenAsync(tokenString);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldNotBeNull();

        _refreshTokensRepositoryMock.Verify(x => x.GetByTokenAsync(tokenString), Times.Once());
    }

    [Fact]
    public async Task GenerateAccesTokenAndRefreshtoken_WithValidUser_ReturnsTokenResponse()
    {
        // Arrange
        var user = new UserAdmin
        {
            Id = 1,
            Email = "test@cafedebug.com",
            Name = "Test User",
            HashedPassword = "hashedpassword"
        };

        _refreshTokensRepositoryMock.Setup(x => x.GetByTokenByUserIdAsync(user.Id))
            .ReturnsAsync((RefreshTokens)null);
        _refreshTokensRepositoryMock.Setup(x => x.SaveAsync(It.IsAny<RefreshTokens>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _jwtService.GenerateAccesTokenAndRefreshtoken(user);

        // Assert
        result.ShouldNotBeNull();
        result.AccessToken.ShouldNotBeNullOrEmpty();
        result.RefreshToken.ShouldNotBeNull();
        result.RefreshToken.Token.ShouldNotBeNullOrEmpty();
        result.TokenType.ShouldBe("Bearer");
        result.ExpiresIn.ShouldBeGreaterThan(0);

        _refreshTokensRepositoryMock.Verify(x => x.GetByTokenByUserIdAsync(user.Id), Times.Once());
    }

    [Fact]
    public async Task RefreshTokenAsync_WhenUserNotFoundForToken_ReturnsFailureResult()
    {
        // Arrange
        var refreshTokenString = "valid-refresh-token";
        var now = _timeProvider.GetUtcNow().UtcDateTime;
        var refreshToken = RefreshTokens.Create(999, "Test User", refreshTokenString, now.AddDays(1), now, now);

        _refreshTokensRepositoryMock.Setup(x => x.GetByTokenAsync(refreshTokenString))
            .ReturnsAsync(refreshToken);
        _refreshTokensRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<RefreshTokens>()))
            .Returns(Task.CompletedTask);
        _userRepositoryMock.Setup(x => x.GetByIdAsync(999))
            .ReturnsAsync((UserAdmin)null);

        // Act
        var result = await _jwtService.RefreshTokenAsync(refreshTokenString);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldNotBeNull();
        result.Error.Code.ShouldBe(nameof(ErrorType.ResourceNotFound));

        _refreshTokensRepositoryMock.Verify(x => x.GetByTokenAsync(refreshTokenString), Times.Once());
        _refreshTokensRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<RefreshTokens>()), Times.Once());
        _userRepositoryMock.Verify(x => x.GetByIdAsync(999), Times.Once());
    }
}
