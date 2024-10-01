using cafedebug.backend.application.Request;
using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respositories;
using cafedebug_backend.domain.Interfaces.Services;
using cafedebug_backend.domain.Request;
using cafedebug_backend.domain.Shared;
using Microsoft.Extensions.Logging;

namespace cafedebug.backend.application.Service
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public AccountService(ILogger<AccountService> logger, IUserRepository userRepository, IEmailService emailService)
        {
            _logger = logger;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task<Result> GeneratePasswordResetToken(SendEmailRequest sendEmailRequest)
        {
            try
            {

                
                await _emailService.SendEmail(sendEmailRequest);
                
                return Result.Success();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Result<UserAdmin>> GetUserAdminByEmail(string email)
        {
            try
            {
                var userAdmin = _userRepository.GetByEmailAsync(email);
                return Result<UserAdmin>.Success(userAdmin.Result);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
