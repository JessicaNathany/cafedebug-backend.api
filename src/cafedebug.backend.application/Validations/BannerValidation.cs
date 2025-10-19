using cafedebug_backend.domain.Banners;
using FluentValidation;

namespace cafedebug.backend.application.Validations
{
    public class BannerValidation : AbstractValidator<Banner>
    {
        public BannerValidation()
        {
            RuleFor(n => n.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(2, 50)
                .WithMessage("The Name field must have between {MinLength} e {MaxLenght} characters");

            RuleFor(b=> b.StartDate)
                .LessThanOrEqualTo(b=> b.EndDate)
                .WithMessage("The start date cannot be greater than the end date.");
        }
    }
}
