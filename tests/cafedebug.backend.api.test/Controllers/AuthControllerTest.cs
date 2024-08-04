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

            _jwtServiceMock.Setup(x => x.GenerateToken(user)).ReturnsAsync(jwtToken);

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

             _jwtServiceMock.Setup(x => x.GenerateToken(user)).ReturnsAsync((JWTToken)null); 

            // Act
            var result = await _authController.GetToken(userCredentials, default);

            // Assert
            var badResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("User unauthorized.", badResult.Value);

        }


        [Fact]
        public async Task GetToken_WhenUserTokenInvalid_ReturnsUnauthorized()
        {

        }

        [Fact]
        public async Task RefreshToken()
        {

        }
    }
}
