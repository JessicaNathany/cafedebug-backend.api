using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Interfaces.Repositories;
using Moq;
using System.Linq.Expressions;

namespace cafedebug.backend.api.test.Shared.Verifications.Account
{
    public class UserRepositoryVerification(Mock<IUserRepository> userRepository)
    {
        public void VerifyUserExists(Times times)
        {
            userRepository.Verify(b => b.AnyAsync(It.IsAny<Expression<Func<UserAdmin, bool>>>()), times);
        }

        public void VerifyGetUserByEmail(string email, Times times)
        {
            userRepository.Verify(x => x.GetByEmailAsync(email), times);
        }

        public void VerifyGetUserByEmail(Times times)
        {
            userRepository.Verify(x => x.GetByEmailAsync(It.IsAny<string>()), times);
        }

        public void VerifyUserSaved(Times times)
        {
            userRepository.Verify(x => x.SaveAsync(It.IsAny<UserAdmin>()), times);
        }

        public void VerifyUserUpdated(Times times)
        {
            userRepository.Verify(x => x.UpdateAsync(It.IsAny<UserAdmin>()), times);
        }
    }
}
