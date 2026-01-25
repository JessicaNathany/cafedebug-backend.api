using cafedebug.backend.application.Accounts.DTOs.Requests;
using cafedebug.backend.application.Accounts.Interfaces;
using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Accounts.Errors;
using cafedebug_backend.domain.Accounts.Services;
using cafedebug_backend.domain.Interfaces.Repositories;
using cafedebug_backend.domain.Messages.Email.Request;
using cafedebug_backend.domain.Messages.Email.Services;
using cafedebug_backend.domain.Shared;
using cafedebug_backend.infrastructure.Constants;
using Microsoft.AspNetCore.Identity;

namespace cafedebug.backend.application.Accounts.Services;

/// <summary>
/// Class responsible for the business rules of account user
/// </summary>
public class AccountService(
    IEmailService emailService, 
    IUserRepository userRepository, 
    IPasswordHasher<UserAdmin> passwordHasher,
    IJWTService jWTService) : IAccountService
{
    public async Task<Result> SendEmailForgotPassword(SendEmailRequest sendEmailRequest)
    {
        await emailService.SendEmail(sendEmailRequest);
        return Result.Success();
    }

    public async Task<Result> ResetPassword(ChangePasswordRequest request)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);

        if (user is null)
            return Result.Failure(UserError.NotFound(request.Email));

        var hashedPassword = passwordHasher.HashPassword(null, request.NewPassword);

        user.Email = request.Email;
        user.HashedPassword = hashedPassword;
        user.LastUpdate = DateTime.Now;

        return Result.Success();
    }

    public async Task<Result> ChangePassword(ChangePasswordRequest request)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);

        if (user is null)
            return Result.Failure(UserError.NotFound(request.Email));

        var hashedPassword = passwordHasher.HashPassword(null, request.NewPassword);

        user.HashedPassword = hashedPassword;

        await userRepository.UpdateAsync(user);
        await userRepository.SaveAsync(user);

        return Result.Success();
    }

    public async Task<Result> ForgotPassword(ForgotPasswordRequest request)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);

        if (user is null)
            return Result.Failure(UserError.NotFound(request.Email));

        var resetToken = jWTService.GenerateResetToken(user.Id);
        var urlResetPassword = InsfrastructureConstants.ForgotPasswordUrl;
        var resetUrl = $"{urlResetPassword}?token={resetToken}";

        var sendEmail = new SendEmailRequest
        {
            Name = request.Name,
            EmailFrom = request.Email,
            Subject = "Reset Password",
            MessageType = InsfrastructureConstants.EmailMessageTypeResetPassword,
            EmailTo = request.Email,
            EmailCopy = "support@cafedebug.com"
        };

        await emailService.SendEmail(sendEmail);
        return Result.Success();
    }

    public async Task<Result> VerifyEmail(string email)
    {
        throw new NotImplementedException();
        // will be to implement email verification in the future ForgotPassword
    }
}