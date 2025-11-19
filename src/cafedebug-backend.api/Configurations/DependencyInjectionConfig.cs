using cafedebug.backend.application.Service;
using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Accounts.Services;
using cafedebug_backend.domain.Jwt;
using cafedebug_backend.domain.Messages.Email.Services;
using cafedebug_backend.infrastructure.Email;
using cafedebug_backend.infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace cafedebug_backend.api.Configurations;

public static class DependencyInjectionConfig
{
    public static void ResolveDependencies(this IServiceCollection service)
    {
        #region Services

        service.AddScoped<IJWTService, JWTService>();
        service.AddScoped<IEmailService, EmailService>();
        service.AddScoped<IEmailSender, SmtpEmailSender>();

        #endregion
        
        #region Others

        AddAuthorizationConfiguration(service);

        #endregion
    }

    private static IServiceCollection AddAuthorizationConfiguration(this IServiceCollection service)
    {
        service.AddSingleton<JwtSettings>();
        service.AddScoped<IPasswordHasher<UserAdmin>, PasswordHasher<UserAdmin>>();

        var jwtSettings = service.BuildServiceProvider().GetRequiredService<JwtSettings>();

        service.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(jwtBearerOptions => jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateActor = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = jwtSettings.SigningCredentials.Key
        });

        service.AddAuthorization();
        return service;
    }
}