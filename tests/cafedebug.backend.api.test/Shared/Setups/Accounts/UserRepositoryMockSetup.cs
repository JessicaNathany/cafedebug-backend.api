using cafedebug.backend.application.Common.Pagination;
using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Banners;
using cafedebug_backend.domain.Banners.Repositories;
using cafedebug_backend.domain.Interfaces.Repositories;
using cafedebug_backend.domain.Shared;
using Moq;
using System.Linq.Expressions;

namespace cafedebug.backend.api.test.Shared.Setups.Accounts
{
    public class UserRepositoryMockSetup(Mock<IUserRepository> userRepository)
    {
        public void UserExists()
        {
            userRepository
                .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<UserAdmin, bool>>>()))
                .ReturnsAsync(true);
        }

        public void GetUserByEmailNotFound(string email)
        {
            userRepository
                .Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserAdmin?)null);
        }

        public void GetUserByEmail(UserAdmin user)
        {
            userRepository
                .Setup(x => x.GetByEmailAsync(user.Email))
                .ReturnsAsync(user);
        }

        public void ResetPassword(Action<UserAdmin> callback)
        {
            userRepository
                .Setup(x => x.SaveAsync(It.IsAny<UserAdmin>()))
                .Callback(callback)
                .Returns(Task.CompletedTask);
        }

        public void UserUpdateSuccess()
        {
            userRepository
                .Setup(x => x.UpdateAsync(It.IsAny<UserAdmin>()))
                .Returns(Task.CompletedTask);

            userRepository
                .Setup(x => x.SaveAsync(It.IsAny<UserAdmin>()))
                .Returns(Task.CompletedTask);
        }

        public void UserUpdateThrows(Exception exception)
        {
            userRepository
                .Setup(x => x.UpdateAsync(It.IsAny<UserAdmin>()))
                .ThrowsAsync(exception);
        }
    }
}
