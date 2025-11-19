using cafedebug.backend.application.Media.DTOs.Requests;
using FluentValidation;

namespace cafedebug.backend.application.Media.Validators;

public class UploadImageValidator : AbstractValidator<UploadImageRequest>
{
    public UploadImageValidator()
    {
        RuleFor(x => x.Base64)
            .NotEmpty()
            .WithMessage("Base64 image cannot be empty")
            .Must(BeValidBase64)
            .WithMessage("Base64 string is not valid");
        
        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("File name cannot be empty")
            .Must(fileName => !string.IsNullOrWhiteSpace(fileName))
            .WithMessage("File name cannot be whitespace only");
        
        RuleFor(x => x.ImageFolder)
            .IsInEnum()
            .WithMessage("Image folder must be a valid enum value");
    }
    
    private static bool BeValidBase64(string base64String)
    {
        if (string.IsNullOrWhiteSpace(base64String))
        {
            return false;
        }

        // Remove data URI scheme if present (e.g., "data:image/png;base64,")
        var base64Data = base64String;
        if (base64String.Contains(','))
        {
            var parts = base64String.Split(',');
            base64Data = parts.Length > 1 ? parts[1] : parts[0];
        }

        // Check if the length is valid (base64 strings should be divisible by 4)
        if (base64Data.Length % 4 != 0)
        {
            return false;
        }

        try
        {
             Convert.FromBase64String(base64Data);
             return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}