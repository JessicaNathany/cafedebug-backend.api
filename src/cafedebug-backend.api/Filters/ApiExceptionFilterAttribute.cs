using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace cafedebug_backend.api.Filters;

public class ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger,  IHostEnvironment environment) : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        HandleException(context);

        base.OnException(context);
    }
    
    private void HandleException(ExceptionContext context)
    {
        var exception = context.Exception;
        var traceId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;
        
        logger.LogError(
            exception,
            "An unhandled exception occurred. TraceId: {TraceId}, Method: {HttpMethod}, Path: {RequestPath}",
            traceId,
            context.HttpContext.Request.Method,
            context.HttpContext.Request.Path.Value);

        var problemDetails = CreateProblemDetails(context, traceId);

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };

        context.ExceptionHandled = true;
    }
    
    private ProblemDetails CreateProblemDetails(ExceptionContext context, string traceId)
    {
        var statusCode = DetermineStatusCode(context.Exception);
        
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = GetTitle(statusCode),
            Type = GetTypeUrl(statusCode),
            Instance = context.HttpContext.Request.Path,
            Extensions =
            {
                ["traceId"] = traceId
            }
        };

        // Only include exception details in development environment
        if (environment.IsDevelopment())
        {
            problemDetails.Detail = context.Exception.Message;
            problemDetails.Extensions["exceptionType"] = context.Exception.GetType().FullName;
            
            if (context.Exception.StackTrace is not null)
            {
                problemDetails.Extensions["stackTrace"] = context.Exception.StackTrace;
            }

            if (context.Exception.InnerException is not null)
            {
                problemDetails.Extensions["innerException"] = context.Exception.InnerException.Message;
            }
        }
        else
        {
            // Generic message for production
            problemDetails.Detail = "An error occurred while processing your request. Please contact support with the trace ID if the problem persists.";
        }

        return problemDetails;
    }
    
    private static int DetermineStatusCode(Exception exception)
    {
        // Map specific exception types to HTTP status codes
        return exception switch
        {
            ArgumentNullException or ArgumentException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status403Forbidden,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            InvalidOperationException => StatusCodes.Status409Conflict,
            NotImplementedException => StatusCodes.Status501NotImplemented,
            TimeoutException => StatusCodes.Status504GatewayTimeout,
            _ => StatusCodes.Status500InternalServerError
        };
    }
    
    private static string GetTitle(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status400BadRequest => "Bad Request",
            StatusCodes.Status403Forbidden => "Forbidden",
            StatusCodes.Status404NotFound => "Not Found",
            StatusCodes.Status409Conflict => "Conflict",
            StatusCodes.Status501NotImplemented => "Not Implemented",
            StatusCodes.Status504GatewayTimeout => "Gateway Timeout",
            _ => "An error occurred while processing your request."
        };
    }
    
    private static string GetTypeUrl(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status400BadRequest => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            StatusCodes.Status403Forbidden => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            StatusCodes.Status404NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            StatusCodes.Status409Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            StatusCodes.Status501NotImplemented => "https://tools.ietf.org/html/rfc7231#section-6.6.2",
            StatusCodes.Status504GatewayTimeout => "https://tools.ietf.org/html/rfc7231#section-6.6.5",
            _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };
    }
}