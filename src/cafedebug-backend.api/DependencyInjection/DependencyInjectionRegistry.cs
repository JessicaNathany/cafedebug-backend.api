using cafedebug.backend.application.Service;
using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.JWT;
using cafedebug_backend.domain.Interfaces.Respositories;
using cafedebug_backend.domain.Interfaces.Services;
using cafedebug_backend.domain.Jwt;
using cafedebug_backend.infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace cafedebug_backend.api.DependencyInjection
{
    public static class DependencyInjectionRegistry
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
            service.AddScoped<IBannerService, BannerService>();
            service.AddScoped<IEpisodeService, EpisodeService>();
            service.AddScoped<ICategoryService, CategoryService>();
            service.AddScoped<IContactService, ContactService>();
            service.AddScoped<ITeamService, TeamService>();
            service.AddScoped<IUserService, UserService>();
            service.AddScoped<IAdvertisementService, AdvertisementService>();

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
}