using cafedebug_backend.domain.Errors;
using cafedebug_backend.domain.Shared.Errors;
using cafedebug.backend.application.Common.DTOs.Response;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace cafedebug.backend.application.Common.Validations;

public class AutoValidationResultFactory : IFluentValidationAutoValidationResultFactory
{
    private readonly ILogger<AutoValidationResultFactory> logger;

    public AutoValidationResultFactory(ILogger<AutoValidationResultFactory> logger)
    {
        this.logger = logger;
    }

    public Task<IActionResult?> CreateActionResult(
        ActionExecutingContext context,
        ValidationProblemDetails validationProblemDetails,
        IDictionary<IValidationContext, FluentValidation.Results.ValidationResult> validationResults)
    {
        if (validationProblemDetails.Errors == null || !validationProblemDetails.Errors.Any())
        {
            logger.LogWarning(
                "Validation failed with no detailed errors. Method: {HttpMethod}, Path: {RequestPath}, TraceId: {TraceId}",
                context.HttpContext.Request.Method,
                context.HttpContext.Request.Path.Value,
                context.HttpContext.TraceIdentifier);

            return Task.FromResult<IActionResult?>(new BadRequestObjectResult(new ValidationErrorResponse
            {
                Code = nameof(ErrorType.ValidationError),
                Message = "Validation failed",
                Errors = new Dictionary<string, string[]>()
            }));
        }

        var errors = validationProblemDetails.Errors
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value
            );

        logger.LogWarning(
            "Validation failed. Method: {HttpMethod}, Path: {RequestPath}, TraceId: {TraceId}, InvalidFieldCount: {InvalidFieldCount}, InvalidFields: {InvalidFields}",
            context.HttpContext.Request.Method,
            context.HttpContext.Request.Path.Value,
            context.HttpContext.TraceIdentifier,
            errors.Count,
            errors.Keys);

        var response = new ValidationErrorResponse
        {
            Code = nameof(ErrorType.ValidationError),
            Message = "Validation failed",
            Errors = errors
        };

        return Task.FromResult<IActionResult?>(new BadRequestObjectResult(response));
    }
}