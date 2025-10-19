using cafedebug_backend.api.Controllers.Admin;
using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Interfaces.JWT;
using cafedebug_backend.domain.Interfaces.Services;
using cafedebug_backend.domain.Shared;
using cafedebug.backend.application.Accounts.DTOs.Requests;
using cafedebug.backend.application.Accounts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace cafedebug.backend.api.test.Controllers
{
    public class AccountsControllerTest
    {
        private readonly Mock<IUserService> _userService;
        private readonly Mock<IAccountService> _accountService;
        private readonly AccountsController _accountController;
        private readonly Mock<IJWTService> _jwtService;
        public AccountsControllerTest()
        {
            var loggerMock = Mock.Of<ILogger<AuthController>>();
            _userService = new Mock<IUserService>();
            _accountService = new Mock<IAccountService>();
            _jwtService = new Mock<IJWTService>();

            _accountController = new AccountsController(loggerMock, _userService.Object, _accountService.Object, _jwtService.Object);
        }

        [Fact]
        public async Task ForgotPassword_InvalidModel_ReturnBadRequest()
        {
            // Arrange
            var forgotPasswordRequest = new ForgotPasswordRequest { Email = string.Empty};
            _accountController.ModelState.AddModelError("Email", "Name");

            // Act
            var result = await _accountController.ForgotPassword(forgotPasswordRequest);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Model is invalid.", badResult.Value.ToString());
        }

        [Fact]
        public async Task ForgotPassword_UserNotFound_ReturnNotFound()
        {
            // Arrange
            var forgotPasswordRequest = new ForgotPasswordRequest { Email = "user.test@example.com", Name = "Café Debug" };

            var userAdmin = new UserAdmin();
            userAdmin = null;

            _userService.Setup(x => x.GetUserAdminByEmail(forgotPasswordRequest.Email)).Returns(Task.FromResult(Result.Success<UserAdmin>(userAdmin)));

            // Act
            var result = await _accountController.ForgotPassword(forgotPasswordRequest);

            // Assert
            var badResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found.", badResult.Value.ToString());
        }

        [Fact]
        public async Task ForgotPassword_ReturnSuccess()
        {
            // Arrange
            var forgotPasswordRequest = new ForgotPasswordRequest { Email = "debugcafe@gmail.com", Name = "Café Debug" };

            var userAdmin = new UserAdmin()
            {
                Id = 1,
                Code = new Guid(),
                Email = "debugcafe@gmail.com",
                HashedPassword = "cf8676b53315b632ec681f2065d6e3c993c3ebaeb667338658b40983d7ce663e",
                Name = "Café Debug"
            };

            _userService.Setup(x => x.GetUserAdminByEmail(forgotPasswordRequest.Email))
                .Returns(Task.FromResult(Result.Success(userAdmin)));

            // Act
            var result = await _accountController.ForgotPassword(forgotPasswordRequest);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task ChangePassword_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var changePasswordRequest = new ChangePasswordRequest { Email = "", NewPassword = "novaSenha123" };
            _accountController.ModelState.AddModelError("Email", "Email é obrigatório");

            // Act
            var result = await _accountController.ChangePassword(changePasswordRequest);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Model is invalid.", badResult.Value.ToString());
        }

        [Fact]
        public async Task ChangePassword_Success_ReturnsNoContent()
        {
            // Arrange
            var changePasswordRequest = new ChangePasswordRequest { Email = "user@cafedebug.com", NewPassword = "newPassword123" };
            _accountService.Setup(x => x.ChangePassword(changePasswordRequest.Email, changePasswordRequest.NewPassword)).Returns(Task.FromResult(Result.Success()));

            // Act
            var result = await _accountController.ChangePassword(changePasswordRequest);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task ChangePassword_UnauthorizedAccessException_ReturnsUnauthorized()
        {
            // Arrange
            var changePasswordRequest = new ChangePasswordRequest { Email = "user@cafedebug.com", NewPassword = "newPassword123" };
            _accountService.Setup(x => x.ChangePassword(changePasswordRequest.Email, changePasswordRequest.NewPassword)).ThrowsAsync(new UnauthorizedAccessException());

            // Act
            var result = await _accountController.ChangePassword(changePasswordRequest);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task VerifyEmail_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var request = new ChangePasswordRequest { Email = "", NewPassword = "password123" };
            _accountController.ModelState.AddModelError("Email", "Email is required");

            // Act
            var result = await _accountController.VerifyEmail(request);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Model is invalid.", badResult.Value.ToString());
        }

        [Fact]
        public async Task VerifyEmail_Success_ReturnsNoContent()
        {
            // Arrange
            var request = new ChangePasswordRequest { Email = "user@cafedebug.com", NewPassword = "password123" };

            // Act
            var result = await _accountController.VerifyEmail(request);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}

