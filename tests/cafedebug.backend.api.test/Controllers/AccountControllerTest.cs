using cafedebug.backend.application.Request;
using cafedebug_backend.api.Administrator.Controllers;
using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Services;
using cafedebug_backend.domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace cafedebug.backend.api.test.Controllers
{
    public class AccountControllerTest
    {
        private readonly Mock<IUserService> _userService;
        private readonly Mock<IAccountService> _accountService;
        private readonly AccountController _accountController;
        public AccountControllerTest()
        {
            var loggerMock = Mock.Of<ILogger<AuthController>>();
            _userService = new Mock<IUserService>();
            _accountService = new Mock<IAccountService>();
            _accountController = new AccountController(loggerMock, _userService.Object, _accountService.Object);
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

            _userService.Setup(x => x.GetUserAdminByEmail(forgotPasswordRequest.Email)).Returns(Task.FromResult(Result<UserAdmin>.Success()));

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
                .Returns(Task.FromResult(Result<UserAdmin>.Success(userAdmin)));

            // Act
            var result = await _accountController.ForgotPassword(forgotPasswordRequest);

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
