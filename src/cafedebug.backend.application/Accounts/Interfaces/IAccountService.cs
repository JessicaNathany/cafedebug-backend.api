using cafedebug_backend.domain.Messages.Email.Request;
using cafedebug_backend.domain.Shared;

namespace cafedebug.backend.application.Accounts.Interfaces;

public interface IAccountService
{
    Task<Result> SendEmailForgotPassword(SendEmailRequest sendEmailRequest);
    Task<Result> ResetPassword(string email, string newPassword);
    Task<Result> ChangePassword(string email, string newPassword);
    Task<Result> ForgotPassword(string email, string name);
    Task<Result> VerifyEmail(string email);
}