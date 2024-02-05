using cafedebug_backend.domain.Entities;
using FluentValidation;

namespace cafedebug.backend.application.Validations
{
    public class EpisodeValidation : AbstractValidator<Episode>
    {
        public EpisodeValidation()
        {
            RuleFor(n => n.Title)
                .NotEmpty().WithMessage("Name is required.")
                .Length(2, 50)
                .WithMessage("The Name field must have between {MinLength} e {MaxLenght} characters");
        }
    }
}
