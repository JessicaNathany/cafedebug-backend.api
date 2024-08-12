using cafedebug.backend.api.test.Mocks;
using cafedebug.backend.application.Request;
using cafedebug_backend.api.Administrator.Controllers;
using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.JWT;
using cafedebug_backend.domain.Interfaces.Services;
using cafedebug_backend.domain.Jwt;
using cafedebug_backend.domain.Shared;
using Microsoft.AspNetCore.Mvc;
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
            _userServiceMock = new Mock<IUserService>();
            _jwtServiceMock = new Mock<IJWTService>();
            _authController = new AuthController(_userServiceMock.Object, _jwtServiceMock.Object);
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
                .Setup(x => x.GetByEmailAsync(userCredentials.Email, userCredentials.Password, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<UserAdmin>.Success(user));

            _jwtServiceMock.Setup(x => x.GenerateToken(user)).Returns(jwtToken);

            // Act
            var result = await _authController.GetToken(userCredentials, default);

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
            var result = await _authController.GetToken(userCredentials, default);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Email and password must not be empty.", badResult.Value);
        }

        [Fact]
        public async Task GetToken_WhenUserNotFound_ReturnsUnauthorized()
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
                .Setup(x => x.GetByEmailAsync(userCredentials.Email, userCredentials.Password, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<UserAdmin>.Success(user));

             _jwtServiceMock.Setup(x => x.GenerateToken(user)).Returns((JWTToken)null); 

            // Act
            var result = await _authController.GetToken(userCredentials, default);

            // Assert
            var badResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("User unauthorized.", badResult.Value);
        }


        [Fact]
        public async Task GetToken_WhenUserTokenInvalid_ReturnsUnauthorized()
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
                .Setup(x => x.GetByEmailAsync(userCredentials.Email, userCredentials.Password, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<UserAdmin>.Success(user));

            _jwtServiceMock.Setup(x => x.GenerateToken(user)).Returns((JWTToken)null);

            // Act
            var result = await _authController.GetToken(userCredentials, default);

            // Assert
            var badResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("User unauthorized.", badResult.Value);
        }

        [Fact]
        public async Task RefreshToken_TokenInvalid_ReturnsUnauthorized()
        {
            // Arrange
            var refreshToken = new RefreshTokenRequest();
            refreshToken.UserId = 1;
            refreshToken.Token = null;

             // Act
            var result = await _authController.RefreshToken(refreshToken, default);

            // Assert
            var badResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Refresh token cannot be null.", badResult.Value);
        }

        [Fact]
        public async Task RefreshToken_ExpirationDateTime_ReturnsUnauthorized()
        {
            // Arrange
            var refreshTokenRequest = DataMocks.RefreshTokenRequest();

            var fakeRefreshToken = new RefreshTokens("debugcafe@local.com", "fake-refresh-token", DateTime.UtcNow.AddMinutes(-5));
            
            _jwtServiceMock.Setup(service => service.GetByTokenAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<RefreshTokens>.Success(fakeRefreshToken));
            
            // Act
            var result = await _authController.RefreshToken(refreshTokenRequest, default);

            // Assert
            var badResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid or expired refresh token.", badResult.Value);
        }

        [Fact] 
        public async Task RefreshToken_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var refreshTokenRequest = DataMocks.RefreshTokenRequest();

            var fakeRefreshToken = new RefreshTokens("debugcafe@local.com", "fake-refresh-token", DateTime.UtcNow.AddMinutes(5));

            _jwtServiceMock.Setup(service => service.GetByTokenAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<RefreshTokens>.Success(fakeRefreshToken));

            _userServiceMock.Setup(x=> x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<UserAdmin>.Failure("User not found."));    

            // Act
            var result = await _authController.RefreshToken(refreshTokenRequest, default);

            // Assert
            var badResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found.", badResult.Value);
        }

        [Fact]
        public async Task RefreshToken_ReturnSuccess()
        {
            // Arrange
            var refreshTokenRequest = DataMocks.RefreshTokenRequest();
            var refreshToken = DataMocks.RefreshTokenMock();

            var jwtToken = new JWTToken("fake-jwt", refreshToken, "tokenType", 5);

            var fakeRefreshToken = new RefreshTokens("debugcafe@local.com", "fake-refresh-token", DateTime.UtcNow.AddMinutes(5));

            _jwtServiceMock.Setup(service => service.GetByTokenAsync(It.IsAny<string>()))
              .ReturnsAsync(Result<RefreshTokens>.Success(fakeRefreshToken));

            _userServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<UserAdmin>.Success(It.IsAny<UserAdmin>()));

            _jwtServiceMock.Setup(x => x.GenerateToken(It.IsAny<UserAdmin>()))
                .Returns(jwtToken);

            // Act
            var result = await _authController.RefreshToken(refreshTokenRequest, default);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task RefreshToken_WhenTokenIsExpired_ReturnsUnauthorized()
        {
            // Arrange
            var fakeRefreshToken = new RefreshTokens("user@example.com", "token", DateTime.UtcNow.AddMinutes(-5));
            _jwtServiceMock.Setup(service => service.GetByTokenAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<RefreshTokens>.Success(fakeRefreshToken));

            var refreshTokenRequest = new RefreshTokenRequest { Token = "token" };

            // Act
            var result = await _authController.RefreshToken(refreshTokenRequest, default);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}
