﻿using cafedebug.backend.application.Request;
using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Services;
using cafedebug_backend.domain.Request;
using cafedebug_backend.domain.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace cafedebug.backend.application.Service
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly IPasswordHasher<UserAdmin> _passwordHasher;

        public AccountService(
            ILogger<AccountService>
            logger, 
            IEmailService emailService, 
            IUserService userService, 
            IPasswordHasher<UserAdmin> passwordHasher)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public async Task<Result> SendEmailForgotPassword(SendEmailRequest sendEmailRequest)
        {
            try
            {
                var emailValidation = EmailValidation(sendEmailRequest.EmailFrom);

                if (!emailValidation)
                {
                    _logger.LogError("Email invalid. - AccountService - SendEmailForgotPassword");
                    return Result.Failure("Email invalid. - AccountService - SendEmailForgotPassword");
                }

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
                var emailValidation = EmailValidation(email);

                if (!emailValidation)
                {
                    _logger.LogError("Email invalid. - AccountService - ResetPassword");
                    return Result.Failure("Email invalid. - AccountService - ResetPassword");
                }

                var user = _userService.GetUserAdminByEmail(email);   

                if (user.Result.Value is null)
                {
                    _logger.LogInformation("User not found.");
                    return Result.Failure("User not found.");
                }

                var hashedPassword = _passwordHasher.HashPassword(null, newPassword);

                user.Result.Value.Email = email;
                user.Result.Value.HashedPassword = hashedPassword;
                user.Result.Value.LastUpdate = DateTime.Now;

                return Result.Success();
            }
            catch (Exception exception)
            {
                _logger.LogError($"An unexpected error occurred. - AccountService - ResetPassword {exception}");
                throw;
            }   
        }
        private bool EmailValidation(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);

            if (!match.Success) return false;

            return true;
        }
    }
}
