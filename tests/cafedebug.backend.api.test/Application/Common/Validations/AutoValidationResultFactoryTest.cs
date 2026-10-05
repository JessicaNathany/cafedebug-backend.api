using cafedebug.backend.application.Common.DTOs.Response;
using cafedebug.backend.application.Common.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
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
        var result = actionResult.ShouldBeOfType<BadRequestObjectResult>();
        result.StatusCode.ShouldBe(400);
        var body = result.Value.ShouldBeOfType<ValidationErrorResponse>();
        body.Code.ShouldBe("ValidationError");
        body.Message.ShouldBe("Validation failed");
        body.Errors.ShouldContainKey("Email");
        body.Errors["Email"].ShouldBe(["Email is required"]);
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
        var result = actionResult.ShouldBeOfType<BadRequestObjectResult>();
        result.StatusCode.ShouldBe(400);
        var body = result.Value.ShouldBeOfType<ValidationErrorResponse>();
        body.Code.ShouldBe("ValidationError");
        body.Message.ShouldBe("Validation failed");
        body.Errors.ShouldBeEmpty();
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
