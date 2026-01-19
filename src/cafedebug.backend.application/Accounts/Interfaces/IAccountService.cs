using cafedebug.backend.application.Accounts.DTOs.Requests;
using cafedebug_backend.domain.Messages.Email.Request;
using cafedebug_backend.domain.Shared;

namespace cafedebug.backend.application.Accounts.Interfaces;

public interface IAccountService
{
    Task<Result> SendEmailForgotPassword(SendEmailRequest sendEmailRequest);
    Task<Result> ResetPassword(ChangePasswordRequest request);
    Task<Result> ChangePassword(ChangePasswordRequest request);
    Task<Result> ForgotPassword(ForgotPasswordRequest request);
    Task<Result> VerifyEmail(string email);
}