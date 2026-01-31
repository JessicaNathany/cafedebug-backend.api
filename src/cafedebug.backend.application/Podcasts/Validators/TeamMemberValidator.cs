using cafedebug.backend.application.Common.Validations;
using cafedebug.backend.application.Podcasts.DTOs.Requests;
using FluentValidation;

namespace cafedebug.backend.application.Podcasts.Validators;

public class TeamMemberValidator : AbstractValidator<TeamMemberRequest>
{
    public TeamMemberValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters")
            .MinimumLength(3).WithMessage("Name must be at least 3 characters");

        RuleFor(request => request.Nickname)
            .MaximumLength(100).WithMessage("Nickname cannot exceed 100 characters");

        RuleFor(request => request.Email)
            .EmailAddress().WithMessage("Invalid email format")
            .When(request => !string.IsNullOrEmpty(request.Email));

        RuleFor(request => request.Bio)
            .MaximumLength(500).WithMessage("Bio cannot exceed 500 characters");

        RuleFor(request => request.PodcastRole)
            .NotEmpty().WithMessage("Podcast role is required")
            .MaximumLength(100).WithMessage("Podcast role cannot exceed 100 characters");

        RuleFor(request => request.GitHubUrl)
            .IsValidUrl().WithMessage("GitHub URL must be a valid URL format")
            .When(request => !string.IsNullOrEmpty(request.GitHubUrl));

        RuleFor(request => request.InstagramUrl)
            .IsValidUrl().WithMessage("Instagram URL must be a valid URL format")
            .When(request => !string.IsNullOrEmpty(request.InstagramUrl));

        RuleFor(request => request.LinkedInUrl)
            .IsValidUrl().WithMessage("LinkedIn URL must be a valid URL format")
            .When(request => !string.IsNullOrEmpty(request.LinkedInUrl));

        RuleFor(request => request.ProfilePhotoUrl)
            .NotEmpty().WithMessage("Profile photo URL is required")
            .IsValidUrl().WithMessage("Profile photo URL must be a valid URL format")
            .IsImageUrl().WithMessage("Profile photo URL must point to an image file");

        RuleFor(request => request.JobTitle)
            .MaximumLength(100).WithMessage("Job title cannot exceed 100 characters");
    }
}