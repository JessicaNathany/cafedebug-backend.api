using cafedebug.backend.application.Service;
using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Accounts.Services;
using cafedebug_backend.domain.Jwt;
using cafedebug_backend.domain.Messages.Email.Services;
using cafedebug_backend.infrastructure.Email;
using cafedebug_backend.infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Serilog;
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
                    Console.WriteLine($"JWT Auth Failed: {context.Exception.Message}");
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    Console.WriteLine("JWT Token Validated Successfully"); // to implement _logg
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    Console.WriteLine("JWT Challenge triggered");
                    return Task.CompletedTask;
                }
            };
        });

        service.AddAuthorization();
        return service;
    }
}