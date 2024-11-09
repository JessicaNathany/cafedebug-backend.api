using cafedebug_backend.domain.Interfaces.Services;
using cafedebug_backend.domain.Request;
using cafedebug_backend.domain.Shared;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace cafedebug.backend.application.Service
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly IEmailService _emailService;

        public AccountService(ILogger<AccountService> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public async Task<Result> SendEmailForgotPassword(SendEmailRequest sendEmailRequest)
        {
            try
            {
                var emailValidation = EmailValidation(sendEmailRequest.Email);

                if (!emailValidation)
                {
                    _logger.LogError("Email invalid.");
                    return Result.Failure("Email invalid.");
                }

                await _emailService.SendEmail(sendEmailRequest);
                return Result.Success();
            }
            catch (Exception exception)
            {
                _logger.LogError($"An unexpected error occurred. {exception}");
                throw;
            }
        }

        private bool EmailValidation(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);

            if (!match.Success)
                return false;

            return true;
        }
    }
}
