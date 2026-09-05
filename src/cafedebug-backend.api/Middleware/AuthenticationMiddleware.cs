using System.Diagnostics;
using System.Security;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace cafedebug_backend.api.Middleware;

/// <summary>
/// Middleware to handle authentication errors properly.
/// </summary>
public partial class AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            LogUnhandledAuthenticationException(logger, ex, context.Request.Path.Value ?? string.Empty);

            // Check if this is an authentication-related exception
            if (IsAuthenticationException(ex) && !context.Response.HasStarted)
            {
                await HandleAuthenticationExceptionAsync(context, ex);
                return;
            }

            // Re-throw if it's not authentication related
            throw;
        }

        // Handle cases where authentication failed but no exception was thrown
        if (context.Response.StatusCode == 401 && !context.Response.HasStarted)
        {
            await HandleUnauthorizedAsync(context);
        }
    }

    private static bool IsAuthenticationException(Exception ex)
    {
        return ex is UnauthorizedAccessException ||
               ex.Message.Contains("authentication", StringComparison.OrdinalIgnoreCase) ||
               ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase) ||
               ex.Message.Contains("JWT", StringComparison.OrdinalIgnoreCase);
    }

    private async Task HandleAuthenticationExceptionAsync(HttpContext context, Exception ex)
    {
        var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
        
        LogAuthenticationFailure(logger, context.Request.Path.Value ?? string.Empty, ex.Message);

        var problemDetails = new AuthenticationProblemDetails(
            "https://tools.ietf.org/html/rfc7235#section-3.1",
            "Unauthorized",
            StatusCodes.Status401Unauthorized,
            "Authentication failed. Please provide a valid JWT token.",
            context.Request.Path.Value,
            traceId);

        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/problem+json";

        var json = JsonSerializer.Serialize(
            problemDetails,
            AuthenticationJsonSerializerContext.Default.AuthenticationProblemDetails);

        await context.Response.WriteAsync(json);
    }

    private async Task HandleUnauthorizedAsync(HttpContext context)
    {
        var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

        var problemDetails = new AuthenticationProblemDetails(
            "https://tools.ietf.org/html/rfc7235#section-3.1",
            "Unauthorized",
            StatusCodes.Status401Unauthorized,
            "Invalid or missing JWT token",
            context.Request.Path.Value,
            traceId);

        context.Response.ContentType = "application/problem+json";

        var json = JsonSerializer.Serialize(
            problemDetails,
            AuthenticationJsonSerializerContext.Default.AuthenticationProblemDetails);

        await context.Response.WriteAsync(json);
    }

    [LoggerMessage(EventId = 1, Level = LogLevel.Error,
        Message = "Authentication middleware caught an exception for {RequestPath}")]
    private static partial void LogUnhandledAuthenticationException(ILogger logger, Exception exception, string requestPath);

    [LoggerMessage(EventId = 2, Level = LogLevel.Warning,
        Message = "Authentication failed for {RequestPath}: {ErrorMessage}")]
    private static partial void LogAuthenticationFailure(ILogger logger, string requestPath, string errorMessage);
}

internal sealed record AuthenticationProblemDetails(
    string Type,
    string Title,
    int Status,
    string Detail,
    string? Instance,
    string TraceId);

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(AuthenticationProblemDetails))]
internal partial class AuthenticationJsonSerializerContext : JsonSerializerContext;
