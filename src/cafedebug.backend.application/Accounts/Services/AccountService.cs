using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Accounts.Errors;
using cafedebug_backend.domain.Interfaces.Repositories;
using cafedebug_backend.domain.Interfaces.Services;
using cafedebug_backend.domain.Request;
using cafedebug_backend.domain.Shared;
using cafedebug.backend.application.Accounts.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace cafedebug.backend.application.Accounts.Services;

/// <summary>
/// Class responsible for the business rules of account user
/// </summary>
public class AccountService : IAccountService
{
    private readonly ILogger<AccountService> _logger;
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<UserAdmin> _passwordHasher;

    public AccountService(
        ILogger<AccountService>
            logger, 
        IEmailService emailService,
        IUserRepository userRespository, 
        IPasswordHasher<UserAdmin> passwordHasher)
    {
        _logger = logger;
        _emailService = emailService;
        _userRepository = userRespository;
        _passwordHasher =  passwordHasher;
    }

    public async Task<Result> SendEmailForgotPassword(SendEmailRequest sendEmailRequest)
    {
        try
        {
            await _emailService.SendEmail(sendEmailRequest);
            return Result.Success();
        }
        catch (Exception exception)
        {
            _logger.LogError($"An unexpected error occurred. - AccountService - SendEmailForgotPassword {exception}");
            throw;
        }
    }

    public async Task<Result> ResetPassword(string email, string newPassword)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(email);   

            if (user is null)
            {
                _logger.LogInformation("User not found.");
                return Result.Failure(UserError.NotFound);
            }

            var hashedPassword = _passwordHasher.HashPassword(null, newPassword);

            user.Email = email;
            user.HashedPassword = hashedPassword;
            user.LastUpdate = DateTime.Now;

            return Result.Success();
        }
        catch (Exception exception)
        {
            _logger.LogError($"An unexpected error occurred. - AccountService - ResetPassword {exception.Message}");
            throw;
        }   
    }

    public async Task<Result> ChangePassword(string email, string newPassword)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user is null)
            {
                _logger.LogInformation("User not found.");
                return Result.Failure(UserError.NotFound);
            }

            var hashedPassword = _passwordHasher.HashPassword(null, newPassword);   

            user.HashedPassword = hashedPassword;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveAsync(user);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError($"An unexpected error occurred. - AccountService - ChangePassword {ex.Message}");
            throw;
        }
    }
}