using cafedebug_backend.domain.Entities;
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
                .WithMessage("O campo Nome deve ter entre {MinLength} e {MaxLenght} caracteres");

            RuleFor(b=> b.StartDate)
                .LessThanOrEqualTo(b=> b.EndDate)
                .WithMessage("The start date cannot be greater than the end date.");
        }
    }
}
