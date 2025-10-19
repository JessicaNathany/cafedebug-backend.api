using System.Security.Cryptography;
using System.Text;
using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Accounts.Errors;
using cafedebug_backend.domain.Interfaces.Repositories;
using cafedebug_backend.domain.Shared;
using cafedebug.backend.application.Accounts.Interfaces;
using cafedebug.backend.application.Validations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace cafedebug.backend.application.Accounts.Services;

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

    public async Task<Result<UserAdmin>> GetByLoginAndPasswordAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (user is null)
        {
            _logger.LogWarning($"User with {email} not found!");
            return Result.Failure<UserAdmin>(UserError.NotFound);
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, password);

        if (verificationResult != PasswordVerificationResult.Success)
        {
            _logger.LogWarning($"Password verification failed for user  {email}");
            return Result.Failure<UserAdmin>(UserError.InvalidPassword);
        }

        return Result.Success(user);
    }

    public async Task<Result<UserAdmin>> CreateAsync(string email, string password)
    {
        // if (String.IsNullOrEmpty(email))
        // {
        //     _logger.LogInformation($"Email cannot be null or empty.");
        //     return Result<UserAdmin>.Failure("Email cannot be null or empty.");
        // }

        var emailValidator = new EmailValidation();
        var emailValidationResult = emailValidator.Validate(email);

        // if (!emailValidationResult.IsValid)
        // {
        //     _logger.LogWarning($"Email invalid or null.");
        //     return Result<UserAdmin>.Failure(emailValidationResult.Errors[0].ErrorMessage);
        // }

        // if (password is null)
        // {
        //     _logger.LogWarning($"Password cannot be null.");
        //     return Result<UserAdmin>.Failure("Password cannot be null.");
        // }

        var hashedPassword = _passwordHasher.HashPassword(null, password);

        var user = new UserAdmin
        {
            Email = email,
            HashedPassword = hashedPassword,
            Code = new Guid(),
            CreatedDate = DateTime.Now
        };

        await _userRepository.SaveAsync(user);
        _logger.LogInformation($"User saved with success.");

        return Result.Success(user);
    }

    public async Task<Result<UserAdmin>> UpdateAsync(UserAdmin userAdmin)
    {
        // if (userAdmin is null)
        // {
        //     _logger.LogWarning($"User admin cannot be null.");
        //     return Result<UserAdmin>.Failure("User admin cannot be null.");
        // }

        var userAdminRepository = await _userRepository.GetByIdAsync(userAdmin.Id);

        if (userAdminRepository is null)
        {
            _logger.LogWarning($"User admin not found.");
            return Result.Failure<UserAdmin>(UserError.NotFound);
        }

        await _userRepository.UpdateAsync(userAdmin);
        _logger.LogInformation($"User updated with success.");

        return Result.Success(userAdmin);
    }

    public async Task<Result<UserAdmin>> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
        {
            _logger.LogWarning($"User admin not found.");
            return Result.Failure<UserAdmin>(UserError.NotFound);
        }

        return Result.Success(user);
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
        {
            _logger.LogWarning($"User admin not found.");
            return Result.Failure<UserAdmin>(UserError.NotFound);
        }

        await _userRepository.DeleteAsync(user.Id);
        _logger.LogInformation($"User deleted with success.");

        return Result.Success();
    }

    public async Task<Result<UserAdmin>> GetByEmailAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (user is null)
        {
            _logger.LogWarning($"User admin not found.");
            return Result.Failure<UserAdmin>(UserError.NotFound);
        }

        if (!CheckPassword(password, user.HashedPassword))
        {
            _logger.LogWarning($"Password invalid.");
            return Result.Failure<UserAdmin>(UserError.InvalidPassword);
        }

        return Result.Success(user);
    }

    public async Task<Result<UserAdmin>> GetUserAdminByEmail(string email)
    {
        var userAdmin = await _userRepository.GetByEmailAsync(email);
        
        if (userAdmin is null)
            return Result.Failure<UserAdmin>(UserError.NotFound);
        
        return Result.Success(userAdmin);
    }

    private bool CheckPassword(string password, string passwordHash)
    {
        var passwordHashGenerated = GenerateSHA256(password);

        if (passwordHashGenerated == passwordHash)
            return true;

        return false;
    }

    private string GenerateSHA256(string password)
    {
        using (var sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}