using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Erros;
using cafedebug_backend.domain.Interfaces.Respository;
using cafedebug_backend.domain.Interfaces.Services;
using cafedebug_backend.domain.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace cafedebug.backend.application.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IPasswordHasher<UserAdmin> _passwordHasher;
        private readonly IStringLocalizer _localizer;
        public UserService(
            IUserRepository userRepository, 
            IPasswordHasher<UserAdmin> passwordHasher,
            ILogger<UserService> logger,
            IStringLocalizer localizer)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
            _localizer = localizer;
        }

        public async Task<Result<UserAdmin>> GetByLoginAndPasswordAsync(string email, string password, CancellationToken cancellationToken)
        {
            var user =  await _userRepository.GetByEmailAsync(email, cancellationToken);

            if (user is null)
            {
                _logger.LogInformation($"User with {email} not found!");
                return Result<UserAdmin>.Failure("User not found.");
                //return Result<UserAdmin>.Failure(EpisodeError.NotFound(_localizer));  TODO: ajustar isso aqui para pegar do Resource
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, password);

            if (verificationResult != PasswordVerificationResult.Success)
            {
                _logger.LogError($"Password verification failed for user  {email}");
                 return Result<UserAdmin>.Failure($"Password verification failed for user.{email}");
            }

            return Result<UserAdmin>.Success(user);
        }

        public async Task<Result<UserAdmin>> CreateUserAsync(string email, string password)
        {
            if(String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
            {
                _logger.LogInformation($"User or password cannot be null!");
                return Result<UserAdmin>.Failure("User not found.");
            }

            var hashedPassword = _passwordHasher.HashPassword(null, password);

            var user = new UserAdmin { Email = email, HashedPassword = hashedPassword };

            return Result<UserAdmin>.Success(user);
        }
    }
}
