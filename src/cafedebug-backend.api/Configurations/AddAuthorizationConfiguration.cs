using cafedebug_backend.domain.Accounts;
using cafedebug_backend.infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using cafedebug_backend.api.Middleware;

namespace cafedebug_backend.api.Configurations;
public static partial class AddAuthorizationConfiguration
{
    public static void ResolveDependencies(this IServiceCollection service, IConfiguration configuration)
    {
        AddJWTConfiguration(service, configuration);
    }

    private static IServiceCollection AddJWTConfiguration(this IServiceCollection service, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.GetSection("JwtSettings").Bind(jwtSettings);
        
        var signingKey = configuration["JwtSettings:SigningKey"];
        if (string.IsNullOrEmpty(signingKey))
        {
            throw new InvalidOperationException("JWT SigningKey is not configured or is empty");
        }
        
        jwtSettings.ConfigureSigningCredentials(signingKey);
        
        service.AddSingleton(jwtSettings);
        service.AddScoped<IPasswordHasher<UserAdmin>, PasswordHasher<UserAdmin>>();

        service.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false; 
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,              
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,         
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = jwtSettings.SigningCredentials?.Key,
                ClockSkew = TimeSpan.Zero             
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();
                    LogAuthenticationFailed(
                        logger,
                        context.Exception?.Message ?? "Unknown error",
                        context.HttpContext.Request.Path.Value ?? string.Empty);
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();
                    LogTokenValidated(
                        logger,
                        context.Principal?.Identity?.Name ?? "Unknown",
                        context.HttpContext.Request.Path.Value ?? string.Empty);
                    return Task.CompletedTask;
                },
                OnChallenge = async context =>
                {
                    // Stop default behavior 
                    context.HandleResponse();

                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();
                    LogAuthenticationChallenge(
                        logger,
                        context.HttpContext.Request.Path.Value ?? string.Empty,
                        context.Error ?? "No specific error",
                        context.ErrorDescription ?? "No description");

                    var traceId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;

                    var problemDetails = new AuthenticationProblemDetails(
                        "https://tools.ietf.org/html/rfc7235#section-3.1",
                        "Unauthorized",
                        StatusCodes.Status401Unauthorized,
                        "Invalid or missing JWT token",
                        context.HttpContext.Request.Path.Value,
                        traceId);

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/problem+json";

                    await context.Response.WriteAsJsonAsync(
                        problemDetails,
                        AuthenticationJsonSerializerContext.Default.AuthenticationProblemDetails);
                },
                OnMessageReceived = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();
                    var hasAuth = context.HttpContext.Request.Headers.ContainsKey("Authorization");
                    LogMessageReceived(
                        logger,
                        context.HttpContext.Request.Path.Value ?? string.Empty,
                        hasAuth);
                    return Task.CompletedTask;
                }
            };
        });

        service.AddAuthorization();
        return service;
    }

    [LoggerMessage(EventId = 1, Level = LogLevel.Error,
        Message = "JWT authentication failed: {ErrorMessage} for {RequestPath}")]
    private static partial void LogAuthenticationFailed(ILogger logger, string errorMessage, string requestPath);

    [LoggerMessage(EventId = 2, Level = LogLevel.Information,
        Message = "JWT token validated successfully for user {UserName} on {RequestPath}")]
    private static partial void LogTokenValidated(ILogger logger, string userName, string requestPath);

    [LoggerMessage(EventId = 3, Level = LogLevel.Error,
        Message = "JWT authentication challenge triggered for {RequestPath}: {Error} - {ErrorDescription}")]
    private static partial void LogAuthenticationChallenge(
        ILogger logger,
        string requestPath,
        string error,
        string errorDescription);

    [LoggerMessage(EventId = 4, Level = LogLevel.Debug,
        Message = "JWT message received for {RequestPath}. Has Authorization header: {HasAuth}")]
    private static partial void LogMessageReceived(ILogger logger, string requestPath, bool hasAuth);
}
