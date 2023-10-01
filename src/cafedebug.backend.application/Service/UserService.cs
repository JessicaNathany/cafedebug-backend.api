using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respository;
using cafedebug_backend.domain.Interfaces.Services;

namespace cafedebug.backend.application.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User> GetByLoginAndPasswordAsync(string login, string Password)
        {
            return _userRepository.GetByLoginAndPassword(login, Password);

            // colocar um validation caso não encontre, exibe a mensagem
        }
    }
}
