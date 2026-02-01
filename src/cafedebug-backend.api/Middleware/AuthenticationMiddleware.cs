using System.Diagnostics;
using System.Security;
using System.Text.Json;

namespace cafedebug_backend.api.Middleware;

/// <summary>
/// Middleware to handle authentication errors properly in .NET 9
/// </summary>
public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthenticationMiddleware> _logger;

    public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Authentication middleware caught an exception for {RequestPath}", 
                context.Request.Path);

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
        
        _logger.LogWarning("Authentication failed for {RequestPath}: {ErrorMessage}", 
            context.Request.Path, ex.Message);

        var problemDetails = new
        {
            type = "https://tools.ietf.org/html/rfc7235#section-3.1",
            title = "Unauthorized",
            status = StatusCodes.Status401Unauthorized,
            detail = "Authentication failed. Please provide a valid JWT token.",
            instance = context.Request.Path.Value,
            traceId = traceId
        };

        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/problem+json";

        var json = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }

    private async Task HandleUnauthorizedAsync(HttpContext context)
    {
        var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

        var problemDetails = new
        {
            type = "https://tools.ietf.org/html/rfc7235#section-3.1",
            title = "Unauthorized",
            status = StatusCodes.Status401Unauthorized,
            detail = "Invalid or missing JWT token",
            instance = context.Request.Path.Value,
            traceId = traceId
        };

        context.Response.ContentType = "application/problem+json";

        var json = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}