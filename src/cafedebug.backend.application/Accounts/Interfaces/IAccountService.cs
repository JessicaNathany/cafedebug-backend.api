using cafedebug_backend.domain.Request;
using cafedebug_backend.domain.Shared;

namespace cafedebug.backend.application.Accounts.Interfaces;

public interface IAccountService
{
    Task<Result> SendEmailForgotPassword(SendEmailRequest sendEmailRequest);

    Task<Result> ResetPassword(string email, string newPassword);

    Task<Result> ChangePassword(string email, string newPassword);
}