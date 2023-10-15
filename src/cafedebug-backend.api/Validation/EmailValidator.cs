using cafedebug_backend.api.ViewModels;
using FluentValidation;
using System.Text.RegularExpressions;

namespace cafedebug_backend.api.Validation
{
    public class EmailValidator : AbstractValidator<UserViewModel>
    {
        public EmailValidator() 
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email address is required.")
            .EmailAddress()
            .Must(IsValidEmail).WithMessage("A valid email address is required.")
            .WithMessage("A valid email address is required.");
        }

        private bool IsValidEmail(string email)
        {
            var regex = new Regex(@"^[\w.-]+@[\w.-]+\.[a-zA-Z]{2,6}$");
            return regex.IsMatch(email);
        }
    }
}
