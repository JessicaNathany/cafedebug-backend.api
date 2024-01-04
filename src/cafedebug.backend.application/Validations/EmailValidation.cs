using FluentValidation;
using System.Text.RegularExpressions;

namespace cafedebug.backend.application.Validations
{
    public class EmailValidation : AbstractValidator<string>
    {
        public EmailValidation()
        {
            RuleFor(x => x)
             .NotNull()
             .WithMessage("Email cannot be null or empty.")
             .NotEmpty();

            RuleFor(x => x)
                .Must(ValidateEmailFormat)
                .When(x => !string.IsNullOrEmpty(x))
                .WithMessage("Email is not in a correct format.");
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
