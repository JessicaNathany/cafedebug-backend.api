using cafedebug.backend.application.Podcasts.DTOs.Requests;
using cafedebug.backend.application.Podcasts.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace cafedebug.backend.api.test.Application.Podcasts.Validators;

[Collection("PodcastTests")]
public class TeamMemberValidatorTests
{
    private readonly TeamMemberValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        // Arrange
        var request = new TeamMemberRequest { Name = "", PodcastRole = "Host", ProfilePhotoUrl = "https://cafedebug.com.br/photo.jpg" };
        
        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Too_Short()
    {
        // Arrange
        var request = new TeamMemberRequest { Name = "Ab", PodcastRole = "Host", ProfilePhotoUrl = "https://cafedebug.com.br/photo.jpg" };
        
        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_PodcastRole_Is_Empty()
    {
        // Arrange
        var request = new TeamMemberRequest { Name = "Jack Doe", PodcastRole = "", ProfilePhotoUrl = "https://cafedebug.com.br/photo.jpg" };
        
        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PodcastRole);
    }

    [Fact]
    public void Should_Have_Error_When_ProfilePhotoUrl_Is_Empty()
    {
        // Arrange
        var request = new TeamMemberRequest { Name = "Jack Doe", PodcastRole = "Host", ProfilePhotoUrl = "" };
        
        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProfilePhotoUrl);
    }

    [Fact]
    public void Should_Have_Error_When_ProfilePhotoUrl_Is_Invalid_Url()
    {
        // Arrange
        var request = new TeamMemberRequest { Name = "Jack Doe", PodcastRole = "Host", ProfilePhotoUrl = "invalid-url" };
        
        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProfilePhotoUrl);
    }

    [Fact]
    public void Should_Have_Error_When_ProfilePhotoUrl_Is_Not_Image()
    {
        // Arrange
        var request = new TeamMemberRequest { Name = "Jack Doe", PodcastRole = "Host", ProfilePhotoUrl = "https://cafedebug.com.br/photo.pdf" };
        
        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProfilePhotoUrl);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        // Arrange
        var request = new TeamMemberRequest 
        { 
            Name = "Jack Doe", 
            PodcastRole = "Host", 
            ProfilePhotoUrl = "https://cafedebug.com.br/photo.jpg",
            Email = "invalid-email"
        };
        
        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_GitHubUrl_Is_Invalid()
    {
        // Arrange
        var request = new TeamMemberRequest 
        { 
            Name = "Jack Doe", 
            PodcastRole = "Host", 
            ProfilePhotoUrl = "https://cafedebug.com.br/photo.jpg",
            GitHubUrl = "invalid-url"
        };
        
        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GitHubUrl);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Request_Is_Valid()
    {
        // Arrange
        var request = new TeamMemberRequest 
        { 
            Name = "Jack Doe", 
            PodcastRole = "Host", 
            ProfilePhotoUrl = "https://cafedebug.com.br/photo.jpg",
            Email = "jack@cafedebug.com.br",
            GitHubUrl = "https://github.com/jackbr"
        };
        
        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}