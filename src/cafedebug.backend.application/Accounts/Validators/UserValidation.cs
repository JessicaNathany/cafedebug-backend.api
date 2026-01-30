using cafedebug.backend.application.Accounts.DTOs.Requests;
using FluentValidation;
using System.Text.RegularExpressions;

namespace cafedebug.backend.application.Accounts.Validators
{
    public class UserValidation : AbstractValidator<UserCredentialsRequest>
    {
        public UserValidation()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .WithMessage("Email cannot be null.")
                .NotEmpty()
                .WithMessage("Email cannot be empty.")
                .Must(ValidateEmailFormat)
                .WithMessage("Email is not in a correct format.");

            RuleFor(x => x.Password)
                .NotNull()
                .WithMessage("Password cannot be null.")
                .NotEmpty()
                .WithMessage("Password cannot be empty.")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.");
        }

        private bool ValidateEmailFormat(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }
    }
}
