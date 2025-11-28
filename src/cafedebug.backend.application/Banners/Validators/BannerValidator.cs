using cafedebug_backend.domain.Banners;
using FluentValidation;

namespace cafedebug.backend.application.Banners.Validators;

public class BannerValidator : AbstractValidator<Banner>
{
    public BannerValidator()
    {
        RuleFor(n => n.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(2, 50)
            .WithMessage("The Name field must have between {MinLength} e {MaxLength} characters");

        RuleFor(b=> b.StartDate)
            .LessThanOrEqualTo(b=> b.EndDate)
            .WithMessage("The start date cannot be greater than the end date.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("StartDate is required");

        RuleFor(x => x.UrlImage)
          .NotEmpty().WithMessage("Image URL is required")
          .Must(BeAnImageUrl).WithMessage("Image URL must point to an image file (jpg, jpeg, png, gif, webp)")
          .MaximumLength(2000).WithMessage("Image URL cannot exceed 2000 characters");
    }

    private static bool BeAnImageUrl(string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            return false;

        var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".svg" };

        if (!Uri.TryCreate(imageUrl, UriKind.Absolute, out var uri)) return false;
        var path = uri.AbsolutePath.ToLowerInvariant();
        return validExtensions.Any(ext => path.EndsWith(ext));
    }
}