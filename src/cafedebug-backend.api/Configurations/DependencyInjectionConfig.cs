using cafedebug.backend.application.Service;
using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Accounts.Repositories;
using cafedebug_backend.domain.Accounts.Services;
using cafedebug_backend.domain.Audience.Repositories;
using cafedebug_backend.domain.Banners.Repositories;
using cafedebug_backend.domain.Episodes.Repositories;
using cafedebug_backend.domain.Interfaces.Repositories;
using cafedebug_backend.domain.Jwt;
using cafedebug_backend.domain.Messages.Email.Services;
using cafedebug_backend.domain.Podcasts.Repositories;
using cafedebug_backend.infrastructure.Data.Repositories;
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
        #region Repositories

        service.AddScoped<IBannerRepository, BannerRepository>();
        service.AddScoped<IEpisodeRepository, EpisodeRepository>();
        service.AddScoped<ICategoryRepository, CategoryRepository>();
        service.AddScoped<IContactRepository, ContactRepository>();
        service.AddScoped<ITeamRepository, TeamRepository>();
        service.AddScoped<IUserRepository, UserRepository>();
        service.AddScoped<IRefreshTokensRepository, RefreshTokensRepository>();

        #endregion

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