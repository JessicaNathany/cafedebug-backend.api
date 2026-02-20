using cafedebug.backend.application.Podcasts.DTOs.Requests;
using cafedebug.backend.application.Podcasts.Validators.Episodes;
using FluentValidation.TestHelper;
using Xunit;

namespace cafedebug.backend.api.test.Application.Podcasts.Validators.Episodes;

public class EpisodeValidatorTests
{
    private readonly EpisodeValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        // Arrange
        var request = new EpisodeRequest { Title = "" };
        
        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_Have_Error_When_Url_Is_Invalid()
    {
        // Arrange
        var request = new EpisodeRequest { Url = "invalid-url" };
        
        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Url)
            .WithErrorMessage("URL must be a valid URL format");
    }

    [Fact]
    public void Should_Have_Error_When_ImageUrl_Is_Invalid_Url()
    {
        // Arrange
        var request = new EpisodeRequest { ImageUrl = "invalid-url" };
        
        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImageUrl)
            .WithErrorMessage("Image URL must be a valid URL format");
    }

    [Fact]
    public void Should_Have_Error_When_ImageUrl_Is_Not_Image()
    {
        // Arrange
        var request = new EpisodeRequest { ImageUrl = "https://example.com/not-an-image.pdf" };
        
        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImageUrl)
            .WithErrorMessage("Image URL must point to an image file (jpg, jpeg, png, gif, webp)");
    }

    [Fact]
    public void Should_Not_Have_Error_When_ImageUrl_Is_Valid_Image_Url()
    {
        // Arrange
        var request = new EpisodeRequest { ImageUrl = "https://example.com/image.png" };
        
        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ImageUrl);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Request_Is_Valid()
    {
        // Arrange
        var request = new EpisodeRequest
        {
            Title = "Valid Episode Title",
            Description = "A very long description that meets the minimum length requirement.",
            ShortDescription = "Short description with enough length.",
            Url = "https://cafedebug.com.br/episodes/1",
            ImageUrl = "https://cafedebug.com.br/images/episode1.jpg",
            PublishedAt = DateTime.UtcNow,
            Number = 1,
            CategoryId = 1,
            DurationInSeconds = 3600
        };
        
        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
