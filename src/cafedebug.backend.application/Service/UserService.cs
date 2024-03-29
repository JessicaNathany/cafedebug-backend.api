using cafedebug.backend.application.Validations;
using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respositories;
using cafedebug_backend.domain.Interfaces.Services;
using cafedebug_backend.domain.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace cafedebug.backend.application.Service
{
    /// <summary>
    /// User admin service: class responsible for the business rules of the user admin to access admin area.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IPasswordHasher<UserAdmin> _passwordHasher;

        public UserService(IUserRepository userRepository,
            IPasswordHasher<UserAdmin> passwordHasher,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<Result<UserAdmin>> GetByLoginAndPasswordAsync(string email, string password, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

            if (user is null)
            {
                _logger.LogWarning($"User with {email} not found!");
                return Result<UserAdmin>.Failure("User not found.");
                //return Result<UserAdmin>.Failure(EpisodeError.NotFound(_localizer));  TODO: ajustar isso aqui para pegar do Resource
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, password);

            if (verificationResult != PasswordVerificationResult.Success)
            {
                _logger.LogWarning($"Password verification failed for user  {email}");
                return Result<UserAdmin>.Failure($"Password verification failed for user.{email}");
            }

            try
            {
                return Result<UserAdmin>.Success(user);
            }
            catch (Exception exception)
            {
                _logger.LogError($"An unexpected error occurred. {exception}");
                return Result<UserAdmin>.Failure("An unexpected error occurred.");
            }
        }

        public async Task<Result<UserAdmin>> CreateAsync(string email, string password, CancellationToken cancellationToken)
        {
            if (String.IsNullOrEmpty(email))
            {
                _logger.LogInformation($"Email cannot be null or empty.");
                return Result<UserAdmin>.Failure("Email cannot be null or empty.");
            }

            var emailValidator = new EmailValidation();
            var emailValidationResult = emailValidator.Validate(email);

            if (!emailValidationResult.IsValid)
            {
                _logger.LogWarning($"Email invalid or null.");
                return Result<UserAdmin>.Failure(emailValidationResult.Errors[0].ErrorMessage);
            }

            if (password is null)
            {
                _logger.LogWarning($"Password cannot be null.");
                return Result<UserAdmin>.Failure("Password cannot be null.");
            }

            try
            {
                var hashedPassword = _passwordHasher.HashPassword(null, password);

                var user = new UserAdmin
                {
                    Email = email,
                    HashedPassword = hashedPassword,
                    Code = new Guid()
                };

                await _userRepository.SaveAsync(user, cancellationToken);
                _logger.LogInformation($"User saved with success.");

                return Result<UserAdmin>.Success(user);
            }
            catch (Exception exception)
            {
                _logger.LogError($"An unexpected error occurred. {exception}");
                return Result<UserAdmin>.Failure("An unexpected error occurred.");
            }
        }

        public async Task<Result<UserAdmin>> UpdateAsync(UserAdmin userAdmin, CancellationToken cancellationToken)
        {
            try
            {
                if (userAdmin is null)
                {
                    _logger.LogWarning($"User admin cannot be null.");
                    return Result<UserAdmin>.Failure("User admin cannot be null.");
                }

                var userAdminRepository = await _userRepository.GetByIdAsync(userAdmin.Id, cancellationToken);

                if (userAdminRepository is null)
                {
                    _logger.LogWarning($"User admin not found.");
                    return Result<UserAdmin>.Failure("User admin not found.");
                }

                await _userRepository.UpdateAsync(userAdmin, cancellationToken);
                _logger.LogInformation($"User updated with success.");

                return Result<UserAdmin>.Success(userAdmin);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred. {ex}");
                return Result<UserAdmin>.Failure("An unexpected error occurred.");
            }
        }

        public async Task<Result<UserAdmin>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id, cancellationToken);

                if (user is null)
                {
                    _logger.LogWarning($"User admin not found.");
                    return Result<UserAdmin>.Failure("User admin not found.");
                }

                return Result<UserAdmin>.Success(user);

            }
            catch (Exception exception)
            {
                _logger.LogError($"An unexpected error occurred. {exception}");
                return Result<UserAdmin>.Failure("An unexpected error occurred.");
            }
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id, cancellationToken);

                if (user is null)
                {
                    _logger.LogWarning($"User admin not found.");
                    return Result.Failure("User admin not found.");
                }

                await _userRepository.DeleteAsync(user.Id, cancellationToken);
                _logger.LogInformation($"User deleted with success.");

                return Result.Success();
            }
            catch (Exception exception)
            {
                _logger.LogError($"An unexpected error occurred. {exception}");
                return Result.Failure("An unexpected error occurred.");
            }       
        }

        public async Task<Result<UserAdmin>> GettByEmailAsync(string email, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

                if(user is null)
                {
                    _logger.LogWarning($"User admin not found.");
                    return Result<UserAdmin>.Failure("User admin not found.");
                }

                return Result<UserAdmin>.Success(user);
            }
            catch (Exception exception)
            {
                _logger.LogError($"An unexpected error occurred. {exception}");
                return Result<UserAdmin>.Failure("An unexpected error occurred.");
            }   
        }
    }
}
