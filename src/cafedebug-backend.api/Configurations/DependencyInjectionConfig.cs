using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Accounts.Repositories;
using cafedebug_backend.domain.Advertisements.Repositories;
using cafedebug_backend.domain.Audience.Repositories;
using cafedebug_backend.domain.Banners.Repositories;
using cafedebug_backend.domain.Episodes.Repositories;
using cafedebug_backend.domain.Interfaces.JWT;
using cafedebug_backend.domain.Interfaces.Repositories;
using cafedebug_backend.domain.Interfaces.Respositories;
using cafedebug_backend.domain.Interfaces.Services;
using cafedebug_backend.domain.Jwt;
using cafedebug_backend.domain.Podcasts.Repositories;
using cafedebug_backend.infrastructure.Data.Repositories;
using cafedebug.backend.application.Accounts;
using cafedebug.backend.application.Accounts.Interfaces;
using cafedebug.backend.application.Accounts.Services;
using cafedebug.backend.application.Advertisements.Interfaces;
using cafedebug.backend.application.Advertisements.Services;
using cafedebug.backend.application.Audience.Interfaces;
using cafedebug.backend.application.Audience.Services;
using cafedebug.backend.application.Banners;
using cafedebug.backend.application.Banners.Interfaces;
using cafedebug.backend.application.Podcasts.Interfaces.Categories;
using cafedebug.backend.application.Podcasts.Interfaces.Episodes;
using cafedebug.backend.application.Podcasts.Interfaces.Teams;
using cafedebug.backend.application.Podcasts.Services.Categories;
using cafedebug.backend.application.Podcasts.Services.Episodes;
using cafedebug.backend.application.Podcasts.Services.Teams;
using cafedebug.backend.application.Service;
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
        service.AddScoped<IAdvertisementRepository, AdvertisementRepository>();
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