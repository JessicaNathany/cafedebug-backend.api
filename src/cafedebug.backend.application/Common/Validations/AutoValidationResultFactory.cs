using cafedebug_backend.domain.Errors;
using cafedebug_backend.domain.Shared.Errors;
using cafedebug.backend.application.Common.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace cafedebug.backend.application.Common.Validations;

public class AutoValidationResultFactory : IFluentValidationAutoValidationResultFactory
{
    public IActionResult CreateActionResult(
        ActionExecutingContext context, 
        ValidationProblemDetails? validationProblemDetails)
    {
        if (validationProblemDetails?.Errors == null || !validationProblemDetails.Errors.Any())
        {
            return new BadRequestObjectResult(new ValidationErrorResponse
            {
                Code = nameof(ErrorType.ValidationError),
                Message = "Validation failed",
                Errors = new Dictionary<string, string[]>()
            });
        }
        
        var errors = validationProblemDetails.Errors
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value
            );

        var response = new ValidationErrorResponse
        {
            Code = nameof(ErrorType.ValidationError),
            Message = "Validation failed",
            Errors = errors
        };

        return new BadRequestObjectResult(response);
    }
}