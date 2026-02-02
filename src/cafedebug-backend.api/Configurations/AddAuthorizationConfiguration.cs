using cafedebug_backend.domain.Accounts;
using cafedebug_backend.infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace cafedebug_backend.api.Configurations;
public static class AddAuthorizationConfiguration
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
                    logger.LogError("JWT authentication failed: {ErrorMessage} for {RequestPath}", 
                        context.Exception?.Message ?? "Unknown error", 
                        context.HttpContext.Request.Path);
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();
                    logger.LogInformation("JWT token validated successfully for user {UserName} on {RequestPath}", 
                        context.Principal?.Identity?.Name ?? "Unknown", 
                        context.HttpContext.Request.Path);
                    return Task.CompletedTask;
                },
                OnChallenge = async context =>
                {
                    // Stop default behavior 
                    context.HandleResponse();

                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();
                    logger.LogError("JWT authentication challenge triggered for {RequestPath}: {Error} - {ErrorDescription}", 
                        context.HttpContext.Request.Path,
                        context.Error ?? "No specific error",
                        context.ErrorDescription ?? "No description");

                    var traceId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;

                    var problemDetails = new
                    {
                        type = "https://tools.ietf.org/html/rfc7235#section-3.1",
                        title = "Unauthorized",
                        status = StatusCodes.Status401Unauthorized,
                        detail = "Invalid or missing JWT token",
                        instance = context.HttpContext.Request.Path.Value,
                        traceId = traceId
                    };

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/problem+json";

                    await context.Response.WriteAsJsonAsync(problemDetails);
                },
                OnMessageReceived = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();
                    var hasAuth = context.HttpContext.Request.Headers.ContainsKey("Authorization");
                    logger.LogDebug("JWT message received for {RequestPath}. Has Authorization header: {HasAuth}", 
                        context.HttpContext.Request.Path, hasAuth);
                    return Task.CompletedTask;
                }
            };
        });

        service.AddAuthorization();
        return service;
    }
}