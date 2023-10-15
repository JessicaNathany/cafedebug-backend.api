using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respository;
using cafedebug_backend.domain.Interfaces.Services;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace cafedebug.backend.application.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IPasswordHasher<UserAdmin> _passwordHasher;
        public UserService(IUserRepository userRepository, IPasswordHasher<UserAdmin> passwordHasher, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<UserAdmin> GetByLoginAndPasswordAsync(string email, string password, CancellationToken cancellationToken)
        {
            var user =  await _userRepository.GetByEmailAsync(email, cancellationToken);

            if (user is null)
            {
                _logger.LogInformation($"User with {email} not found!");
                return null;
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, password);

            if (verificationResult != PasswordVerificationResult.Success)
            {
                _logger.LogError($"Password verification failed for user  {email}");
                return null;
            }

            return user;
        }

        public async Task<UserAdmin> CreateUserAsync(string email, string password)
        {
            var hashedPassword = _passwordHasher.HashPassword(null, password);

            var user = new UserAdmin { Email = email, HashedPassword = hashedPassword };

            return await _userRepository.SaveAsync(user);
        }
    }
}
