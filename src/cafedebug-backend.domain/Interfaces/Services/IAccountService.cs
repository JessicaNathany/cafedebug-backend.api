using cafedebug_backend.domain.Request;
using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Interfaces.Services
{
    public interface IAccountService
    {
        Task<Result> SendEmailForgotPassword(SendEmailRequest sendEmailRequest);
    }
}
