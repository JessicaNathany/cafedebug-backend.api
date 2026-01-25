using cafedebug.backend.application.Accounts.Interfaces;
using cafedebug.backend.application.Accounts.Services;
using cafedebug.backend.application.Service;
using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Accounts.Services;
using cafedebug_backend.domain.Messages.Email.Services;
using cafedebug_backend.infrastructure.Email;
using cafedebug_backend.infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace cafedebug_backend.api.Configurations;

public static class DependencyInjectionConfig
{
    
    public static void ResolveDependencies(this IServiceCollection service, IConfiguration configuration)
    {
        #region Services

        service.AddScoped<IJWTService, JWTService>();
        service.AddScoped<IEmailService, EmailService>();
        service.AddScoped<IEmailSender, SmtpEmailSender>();
        service.AddScoped<IAuthService, AuthService>();
        service.AddScoped<IAccountService, AccountService>();
        service.AddScoped<IUserService, UserService>();

        #endregion
        
        #region Others

        AddAuthorizationConfiguration(service, configuration);

        #endregion
    }

    private static IServiceCollection AddAuthorizationConfiguration(this IServiceCollection service, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.GetSection("JwtSettings").Bind(jwtSettings);
        
        var signingKey = configuration["JwtSettings:SigningKey"];
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
                IssuerSigningKey = jwtSettings.SigningCredentials.Key,
                ClockSkew = TimeSpan.Zero             
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();
                    logger.LogWarning("JWT authentication failed: {ErrorMessage} for {RequestPath}", 
                        context.Exception.Message, 
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
                OnChallenge = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();
                    logger.LogWarning("JWT authentication challenge triggered for {RequestPath}: {Error} - {ErrorDescription}", 
                        context.HttpContext.Request.Path,
                        context.Error ?? "No specific error",
                        context.ErrorDescription ?? "No description");
                    return Task.CompletedTask;
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