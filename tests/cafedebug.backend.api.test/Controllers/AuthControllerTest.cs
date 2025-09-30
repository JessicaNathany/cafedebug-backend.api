using cafedebug.backend.api.test.Mocks;
using cafedebug.backend.application.Request;
using cafedebug_backend.api.Administrator.Controllers;
using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.JWT;
using cafedebug_backend.domain.Interfaces.Services;
using cafedebug_backend.domain.Jwt;
using cafedebug_backend.domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace cafedebug.backend.api.test.Controllers
{
    public class AuthControllerTest
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IJWTService> _jwtServiceMock;
        private readonly AuthController _authController;
        public AuthControllerTest()
        {
            var loggerMock = Mock.Of<ILogger<AuthController>>();
            _userServiceMock = new Mock<IUserService>();
            _jwtServiceMock = new Mock<IJWTService>();

            _authController = new AuthController(loggerMock, _userServiceMock.Object, _jwtServiceMock.Object);
        }

        [Fact]
        public async Task GetToken_WhenUserIsValid_ReturnsOkObjectResultWithToken()
        {
            // Arrange
            var userCredentials = DataMocks.UserRequest();
            var user = DataMocks.UserAdminMock();

            var refreshToken = DataMocks.RefreshTokenMock();

            var jwtToken = JWTToken.Create(
                accessToken: "fake-jwt-token",
                refreshToken: refreshToken,
                tokenType: "Bearer",
                expiresIn: 900 // 15 minutes
            );

            _userServiceMock
                .Setup(x => x.GetByEmailAsync(userCredentials.Email, userCredentials.Password)).ReturnsAsync(Result<UserAdmin>.Success(user));

            _jwtServiceMock.Setup(x => x.GenerateAccesTokenAndRefreshtoken(user)).ReturnsAsync(jwtToken);

            // Act
            var result = await _authController.GenerateToken(userCredentials);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedToken = Assert.IsType<JWTToken>(okResult.Value);
            Assert.Equal("fake-jwt-token", returnedToken.AccessToken);
            Assert.Equal("fake-refresh-token", returnedToken.RefreshToken.Token);
            Assert.Equal("Bearer", returnedToken.TokenType);
            Assert.Equal(900, returnedToken.ExpiresIn);
        }

        [Fact]
        public async Task GetToken_WhenCredentialsAreEmpty_ReturnsUnauthorized()
        {
            // Arrange
            var userCredentials = new UserCredentialsRequest { Email = "", Password = "" };

            // Act
            var result = await _authController.GenerateToken(userCredentials);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Email and password must not be empty.", badResult.Value);
        }

        [Fact]
        public async Task GetToken_WhenUserNotFound_ReturnsBadRequest()
        {
            // Arrange
            var userCredentials = DataMocks.UserRequest();
            var user = DataMocks.UserAdminMock();

            var refreshToken = DataMocks.RefreshTokenMock();

            var jwtToken = JWTToken.Create(
                accessToken: "fake-jwt",
                refreshToken: refreshToken,
                tokenType: "Bearer",
                expiresIn: 900 // 15 minutes
            );

            _userServiceMock
                .Setup(x => x.GetByEmailAsync(userCredentials.Email, userCredentials.Password))
                .ReturnsAsync(Result<UserAdmin>.Success(user));

            _jwtServiceMock.Setup(x => x.GenerateAccesTokenAndRefreshtoken(user)).ReturnsAsync((JWTToken)null); 

            // Act
            var result = await _authController.GenerateToken(userCredentials);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error creating token for user.", badResult.Value);
        }


        [Fact]
        public async Task RefreshToken_TokenInvalid_ReturnsUnauthorized()
        {
            // Arrange
            var refreshToken = new RefreshTokenRequest();
            refreshToken.RefreshToken = null;

             // Act
            var result = await _authController.RefreshToken(refreshToken);

            // Assert
            var badResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Refresh token cannot be null.", badResult.Value);
        }

        [Fact]
        public async Task RefreshToken_ExpirationDateTime_ReturnsUnauthorized()
        {
            // Arrange
            var refreshTokenRequest = DataMocks.RefreshTokenRequest();

            var fakeRefreshToken = new RefreshTokens(1, "debugcafe@local.com", "fake-refresh-token", DateTime.UtcNow.AddMinutes(-5), DateTime.UtcNow.AddMinutes(-5));
            
            _jwtServiceMock.Setup(service => service.GetByTokenAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<RefreshTokens>.Success(fakeRefreshToken));
            
            // Act
            var result = await _authController.RefreshToken(refreshTokenRequest);

            // Assert
            var badResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid or expired refresh token.", badResult.Value);
        }

        [Fact] 
        public async Task RefreshToken_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var refreshTokenRequest = DataMocks.RefreshTokenRequest();

            var fakeRefreshToken = new RefreshTokens(1, "debugcafe@local.com", "fake-refresh-token", DateTime.UtcNow.AddMinutes(5), DateTime.Now);

            _jwtServiceMock.Setup(service => service.GetByTokenAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<RefreshTokens>.Success(fakeRefreshToken));

            _userServiceMock.Setup(x=> x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Result<UserAdmin>.Failure("User not found."));    

            // Act
            var result = await _authController.RefreshToken(refreshTokenRequest);

            // Assert
            var badResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found.", badResult.Value);
        }

        [Fact]
        public async Task RefreshToken_ReturnSuccess()
        {
            // Arrange
            var refreshTokenRequest = DataMocks.RefreshTokenRequest();
            var userAdmin = new UserAdmin
            { 
                Id = 1,
                Name = "Café Debug",
                HashedPassword = "123456",
                Email = "debugcafe@local.com",
                Code = Guid.NewGuid()
            };

            var refreshToken = DataMocks.RefreshTokenMock();

            var jwtToken = new JWTToken("fake-jwt", refreshToken, "tokenType", 5);

            var fakeRefreshToken = new RefreshTokens(
                1, "debugcafe@local.com", "fake-refresh-token", DateTime.UtcNow.AddMinutes(5), DateTime.Now);

            _jwtServiceMock.Setup(service => service.GetByTokenAsync(It.IsAny<string>()))
              .ReturnsAsync(Result<RefreshTokens>.Success(fakeRefreshToken));

            _userServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Result<UserAdmin>.Success(userAdmin));

            _jwtServiceMock.Setup(x => x.GenerateAccesTokenAndRefreshtoken(It.IsAny<UserAdmin>()))
                .ReturnsAsync(jwtToken);

            _jwtServiceMock.Setup(x => x.RefreshTokenAsync(
                It.IsAny<RefreshTokens>(),
                It.IsAny<UserAdmin>())).ReturnsAsync(jwtToken);

            // Act
            var result = await _authController.RefreshToken(refreshTokenRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task RefreshToken_WhenTokenIsExpired_ReturnsUnauthorized()
        {
            // Arrange
            var fakeRefreshToken = new RefreshTokens(1, "user@example.com", "token", DateTime.UtcNow.AddMinutes(-5), DateTime.Now);
            _jwtServiceMock.Setup(service => service.GetByTokenAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<RefreshTokens>.Success(fakeRefreshToken));

            var refreshTokenRequest = new RefreshTokenRequest { RefreshToken = "refreshToken" };

            // Act
            var result = await _authController.RefreshToken(refreshTokenRequest);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid or expired refresh token.", unauthorizedResult.Value);
        }
    }
}
