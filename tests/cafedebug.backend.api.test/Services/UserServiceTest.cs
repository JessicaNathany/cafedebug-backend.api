using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respository;
using cafedebug_backend.domain.Interfaces.Services;
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
            string password = "admin@123";

            // corrigir teste

            var service = _autoMocker.CreateInstance<IUserService>();
            await service.GetByLoginAndPasswordAsync(email, password, It.IsAny<CancellationToken>());

            _autoMocker.GetMock<IUserRepository>()

                .Setup(x => x.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<UserAdmin>(null));

            var result = await service.GetByLoginAndPasswordAsync(email, password, It.IsAny<CancellationToken>());

            Assert.Null(result);
        }
    }
}
