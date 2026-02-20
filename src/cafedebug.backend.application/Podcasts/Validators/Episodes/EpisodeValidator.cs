using cafedebug.backend.application.Common.Validations;
using cafedebug.backend.application.Podcasts.DTOs.Requests;
using FluentValidation;

namespace cafedebug.backend.application.Podcasts.Validators.Episodes;

public class EpisodeValidator : AbstractValidator<EpisodeRequest>
{
    public EpisodeValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(5000).WithMessage("Description cannot exceed 5000 characters")
            .MinimumLength(10).WithMessage("Description must be at least 10 characters");

        RuleFor(x => x.ShortDescription)
            .NotEmpty().WithMessage("Short description is required")
            .MaximumLength(500).WithMessage("Short description cannot exceed 500 characters")
            .MinimumLength(10).WithMessage("Short description must be at least 10 characters");

        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("URL is required")
            .IsValidUrl().WithMessage("URL must be a valid URL format")
            .MaximumLength(2000).WithMessage("URL cannot exceed 2000 characters");

        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("Image URL is required")
            .IsValidUrl().WithMessage("Image URL must be a valid URL format")
            .IsImageUrl().WithMessage("Image URL must point to an image file (jpg, jpeg, png, gif, webp)")
            .MaximumLength(2000).WithMessage("Image URL cannot exceed 2000 characters");

        RuleFor(x => x.Tags)
            .Must(tags => tags is not { Count: > 20 })
            .WithMessage("Cannot have more than 20 tags")
            .Must(tags => tags == null || tags.All(tag => !string.IsNullOrWhiteSpace(tag)))
            .WithMessage("Tags cannot be empty or whitespace")
            .Must(tags => tags == null || tags.All(tag => tag.Length <= 50))
            .WithMessage("Each tag cannot exceed 50 characters");

        RuleFor(x => x.PublishedAt)
            .NotEmpty().WithMessage("Published date is required");

        RuleFor(x => x.Number)
            .GreaterThan(0).WithMessage("Episode number must be greater than 0");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category id is required");

        RuleFor(x => x.DurationInSeconds)
            .GreaterThan(0).WithMessage("Duration must be greater than 0 seconds")
            .LessThanOrEqualTo(86400).WithMessage("Duration cannot exceed 24 hours (86400 seconds)");
    }
}