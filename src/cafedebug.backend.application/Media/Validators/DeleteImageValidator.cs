using cafedebug.backend.application.Media.DTOs.Requests;
using FluentValidation;

namespace cafedebug.backend.application.Media.Validators;

public class DeleteImageValidator : AbstractValidator<DeleteImageRequest>
{
    public DeleteImageValidator()
    {
        RuleFor(x => x.ImageUrl)
            .NotEmpty()
            .WithMessage("Image URL cannot be empty")
            .Must(BeAValidHttpUrl)
            .WithMessage("Image URL must be a valid HTTP or HTTPS URL")
            .Must(BeAValidImageUrl)
            .WithMessage("URL must point to a valid image file (jpg, jpeg, png, gif, webp, svg)")
            .MaximumLength(2000)
            .WithMessage("Image URL cannot exceed 2000 characters");
    }
    
    private static bool BeAValidHttpUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        if (!Uri.TryCreate(url, UriKind.Absolute, out var uriResult))
            return false;

        return uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps;
    }

    private static bool BeAValidImageUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            return false;

        var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".svg" };
        var path = uri.AbsolutePath.ToLowerInvariant();
        
        return validExtensions.Any(ext => path.EndsWith(ext));
    }
}