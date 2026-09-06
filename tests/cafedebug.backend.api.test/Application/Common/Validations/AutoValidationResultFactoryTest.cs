using cafedebug.backend.application.Common.DTOs.Response;
using cafedebug.backend.application.Common.Validations;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace cafedebug.backend.api.test.Application.Common.Validations;

public class AutoValidationResultFactoryTest
{
    private readonly AutoValidationResultFactory _factory = new(new Mock<ILogger<AutoValidationResultFactory>>().Object);

    [Fact]
    public async Task CreateActionResult_WhenErrorsPresent_SetsBadRequestValidationErrorResponse()
    {
        // Arrange
        var context = CreateActionExecutingContext();
        var problemDetails = new ValidationProblemDetails(new Dictionary<string, string[]>
        {
            ["Email"] = ["Email is required"]
        });

        // Act
        var actionResult = await _factory.CreateActionResult(
            context,
            problemDetails,
            new Dictionary<IValidationContext, FluentValidation.Results.ValidationResult>());

        // Assert
        var result = actionResult.Should().BeOfType<BadRequestObjectResult>().Subject;
        result.StatusCode.Should().Be(400);
        var body = result.Value.Should().BeOfType<ValidationErrorResponse>().Subject;
        body.Code.Should().Be("ValidationError");
        body.Message.Should().Be("Validation failed");
        body.Errors.Should().ContainKey("Email");
        body.Errors["Email"].Should().Equal("Email is required");
    }

    [Fact]
    public async Task CreateActionResult_WhenErrorsEmpty_SetsGenericValidationErrorResponse()
    {
        // Arrange
        var context = CreateActionExecutingContext();
        var problemDetails = new ValidationProblemDetails();

        // Act
        var actionResult = await _factory.CreateActionResult(
            context,
            problemDetails,
            new Dictionary<IValidationContext, FluentValidation.Results.ValidationResult>());

        // Assert
        var result = actionResult.Should().BeOfType<BadRequestObjectResult>().Subject;
        result.StatusCode.Should().Be(400);
        var body = result.Value.Should().BeOfType<ValidationErrorResponse>().Subject;
        body.Code.Should().Be("ValidationError");
        body.Message.Should().Be("Validation failed");
        body.Errors.Should().BeEmpty();
    }

    private static ActionExecutingContext CreateActionExecutingContext()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Method = "POST";
        httpContext.Request.Path = "/test";
        httpContext.TraceIdentifier = "trace-id";

        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        return new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            controller: new object());
    }
}
