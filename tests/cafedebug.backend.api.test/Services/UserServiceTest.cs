using cafedebug.backend.application.Service;
using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Repositories;
using cafedebug_backend.domain.Interfaces.Respositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace cafedebug.backend.api.test.Services
{
    public class UserServiceTest
    {
        private readonly AutoMocker _autoMocker;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IStringLocalizer> _localizerMock;
        private readonly Mock<IPasswordHasher<UserAdmin>> _passwordHasherMock;
        private readonly Mock<ILogger<UserService>> _loggerMock;

        public UserServiceTest()
        {
            _autoMocker = new AutoMocker();

            _userRepositoryMock = new Mock<IUserRepository>();
            _localizerMock = new Mock<IStringLocalizer>();
            _passwordHasherMock = new Mock<IPasswordHasher<UserAdmin>>();
        }

        [Fact]
        public async Task GetByLoginAndPassword_ShouldBe_UserNotFound()
        {
            string email = "cafe.teste@gmail.com";
            string password = "123456";

            var service = _autoMocker.CreateInstance<UserService>();
            await service.GetByLoginAndPasswordAsync(email, password);

            _autoMocker.GetMock<IUserRepository>()
                .Setup(x => x.GetByEmailAsync(email))
                .Returns(Task.FromResult<UserAdmin>(null));

            var result = await service.GetByLoginAndPasswordAsync(email, password);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            Assert.Null(result.Value);
            Assert.Equal("User not found.", result.Error);
        }

        [Fact]
        public async Task GetByLoginAndPassword_VerifyHashedPassword_ShouldBe_Success()
        {
            var email = "cafedev@gmail.com";
            var password = "123456";

            var userAdmin = new UserAdmin
            {
                Code = Guid.NewGuid(),
                Name = "café debug",
                Email = email,
                HashedPassword = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92"
            };

            _userRepositoryMock.Setup(x => x.GetByEmailAsync(email)).ReturnsAsync(userAdmin);

            _passwordHasherMock.Setup(x => x.VerifyHashedPassword(It.IsAny<UserAdmin>(), It.IsAny<string>(), password))
                               .Returns(PasswordVerificationResult.Success);

            var looggerMock = Mock.Of<ILogger<UserService>>();

            var userService = new UserService(_userRepositoryMock.Object, _passwordHasherMock.Object, looggerMock);

            // Act
            var result = await userService.GetByLoginAndPasswordAsync(email, password);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetByLoginAndPassword_VerifyHashedPassword_ShouldBe_Fail()
        {
            var email = "cafedev@teste.com";
            var password = "123456";

            var userAdmin = new UserAdmin
            {
                Code = Guid.NewGuid(),
                Name = "café debug",
                Email = email,
                HashedPassword = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92"
            };

            _userRepositoryMock.Setup(x => x.GetByEmailAsync(email)).ReturnsAsync(userAdmin);

            _passwordHasherMock.Setup(x => x.VerifyHashedPassword(It.IsAny<UserAdmin>(), It.IsAny<string>(), password))
                               .Returns(PasswordVerificationResult.Failed);

            var looggerMock = Mock.Of<ILogger<UserService>>();

            var userService = new UserService(_userRepositoryMock.Object, _passwordHasherMock.Object, looggerMock);

            // Act
            var result = await userService.GetByLoginAndPasswordAsync(email, password);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal($"Password verification failed for user.{email}", result.Error);
        }

        [Fact]
        public async Task Create_CreateAsync_ShouldBe_EmailNull()
        {
            var password = "123456";

            var looggerMock = Mock.Of<ILogger<UserService>>();
            var userService = new UserService(_userRepositoryMock.Object, _passwordHasherMock.Object, looggerMock);

            // Act
            var result = await userService.CreateAsync(null, password);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal("Email cannot be null or empty.", result.Error);
        }

        [Fact]
        public async Task Create_CreateAsync_ShouldBe_EmailInvalid()
        {
            var email = "cafedevteste.com";
            var password = "123456";

            var userAdmin = new UserAdmin
            {
                Code = Guid.NewGuid(),
                Name = "café debug",
                Email = email,
                HashedPassword = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92"
            };

            _passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<UserAdmin>(), password))
                               .Returns(userAdmin.HashedPassword);

            var looggerMock = Mock.Of<ILogger<UserService>>();
            var userService = new UserService(_userRepositoryMock.Object, _passwordHasherMock.Object, looggerMock);

            // Act
            var result = await userService.CreateAsync(email, password);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal("Email is not in a correct format.", result.Error);
        }

        [Fact]
        public async Task Create_CreateAsync_ShouldBe_PasswordNull()
        {
            var email = "cafede@teste.com";

            var userAdmin = new UserAdmin
            {
                Code = Guid.NewGuid(),
                Name = "café debug",
                Email = email,
                HashedPassword = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92"
            };

            var looggerMock = Mock.Of<ILogger<UserService>>();
            var userService = new UserService(_userRepositoryMock.Object, _passwordHasherMock.Object, looggerMock);

            // Act
            var result = await userService.CreateAsync(email, null);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal("Password cannot be null.", result.Error);
        }

        [Fact]
        public async Task Update_UpdateAsync_ShouldBe_Success()
        {
            var userAdmin = new UserAdmin
            {
                Code = Guid.NewGuid(),
                Name = "café debug",
                Email = "cafede@teste.com",
                HashedPassword = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92"
            };

            var looggerMock = Mock.Of<ILogger<UserService>>();
            _userRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(userAdmin));

            var userService = new UserService(_userRepositoryMock.Object, _passwordHasherMock.Object, looggerMock);

            // Act
            var result = await userService.UpdateAsync(userAdmin);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task Update_UpdateAsync_ShouldBe_Error_UserNotFound()
        {
            var userAdmin = new UserAdmin
            {
                Code = Guid.NewGuid(),
                Name = "café debug",
                Email = "cafede@teste.com",
                HashedPassword = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92"
            };

            var looggerMock = Mock.Of<ILogger<UserService>>();
            var userService = new UserService(_userRepositoryMock.Object, _passwordHasherMock.Object, looggerMock);

            var userMock = new Mock<IUserRepository>();
            userMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult<UserAdmin>(null));

            // Act
            var result = await userService.UpdateAsync(userAdmin);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal("User admin not found.", result.Error);
        }

        [Fact]
        public async Task Update_UpdateAsync_ShouldBe_Error_UserNull()
        {
            var looggerMock = Mock.Of<ILogger<UserService>>();
            var userService = new UserService(_userRepositoryMock.Object, _passwordHasherMock.Object, looggerMock);

            // Act
            var result = await userService.UpdateAsync(null);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal("User admin cannot be null.", result.Error);
        }

        [Fact]
        public async Task Update_GetByIUdAsync_ShouldBe_Error_UserNotFound()
        {
            var looggerMock = Mock.Of<ILogger<UserService>>();
            var userService = new UserService(_userRepositoryMock.Object, _passwordHasherMock.Object, looggerMock);

            // Act
            var result = await userService.GetByIdAsync(1);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal("User admin not found.", result.Error);
        }

        [Fact]
        public async Task GetByLoginAndPassword_CreateUserAsync_ShouldBe_Success()
        {
            var email = "cafede@teste.com";
            var password = "123456";

            var userAdmin = new UserAdmin
            {
                Code = Guid.NewGuid(),
                Name = "café debug",
                Email = email,
                HashedPassword = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92"
            };

            _passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<UserAdmin>(), password))
                               .Returns(userAdmin.HashedPassword);

            var looggerMock = Mock.Of<ILogger<UserService>>();
            var userService = new UserService(_userRepositoryMock.Object, _passwordHasherMock.Object, looggerMock);

            // Act
            var result = await userService.CreateAsync(email, password);

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}
