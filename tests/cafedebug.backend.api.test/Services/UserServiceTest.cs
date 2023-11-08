using cafedebug.backend.application.Service;
using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respository;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace cafedebug.backend.api.test.Services
{
    public class UserServiceTest
    {
        private readonly AutoMocker _autoMocker;

        public UserServiceTest()
        {
            _autoMocker = new AutoMocker();
        }

        [Fact]
        public async void GetByLoginAndPassword_ShouldBe_UserNotFound()
        {
            string email = "cafedev@gmail.com";
            string password = "123456";

            var service = _autoMocker.CreateInstance<UserService>();
            await service.GetByLoginAndPasswordAsync(email, password, It.IsAny<CancellationToken>());

            _autoMocker.GetMock<IUserRepository>()
                .Setup(x => x.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<UserAdmin>(null));

            var result = await service.GetByLoginAndPasswordAsync(email, password, It.IsAny<CancellationToken>());

            // Assert
            Assert.False(result.IsSuccess); 
            Assert.NotNull(result.Error);
            Assert.Null(result.Value);
            Assert.Equal("User not found.", result.Error); 
        }

        [Fact]
        public async void GetByLoginAndPassword_VerifyHashedPassword_ShouldBe_Success()
        {
            //string email = "cafedev@gmail.com";
            //string password = "123456";

            //var userAdmin = new UserAdmin
            //{
            //    Email = email,
            //    HashedPassword = password
            //};

            //var service = _autoMocker.CreateInstance<UserService>();
            //await service.GetByLoginAndPasswordAsync(email, password, It.IsAny<CancellationToken>());

            //_autoMocker.GetMock<IUserRepository>()
            //    .Setup(x => x.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
            //    .Returns(Task.FromResult<UserAdmin>(null));

            //var result = await service.GetByLoginAndPasswordAsync(email, password, It.IsAny<CancellationToken>());

            //// Assert
            //Assert.False(result.IsSuccess);
            //Assert.NotNull(result.Error);
            //Assert.Null(result.Value);
            //Assert.Equal("User not found.", result.Error);

            throw new NotImplementedException();
        }
    }
    
}
