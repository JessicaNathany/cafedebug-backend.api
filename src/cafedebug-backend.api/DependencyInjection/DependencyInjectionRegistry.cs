using cafedebug.backend.application.Service;
using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.JWT;
using cafedebug_backend.domain.Interfaces.Respositories;
using cafedebug_backend.domain.Interfaces.Services;
using cafedebug_backend.infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Identity;

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
            service.AddScoped<ITagRepository, TagRepository>();
            service.AddScoped<IAdvertisementRepository, AdvertisementRepository>();

            #endregion

            #region Services

            service.AddScoped<IJWTService, JWTService>();
            service.AddScoped<IBannerService, BannerService>();
            service.AddScoped<IEpisodeService, EpisodeService>();
            service.AddScoped<ICategoryService, CategoryService>();
            service.AddScoped<IContactService, ContactService>();
            service.AddScoped<ITeamService, TeamService>();
            service.AddScoped<IUserService, UserService>();
            service.AddScoped<ITagService, TagService>();
            service.AddScoped<IAdvertisementService, AdvertisementService>();

            #endregion

            #region Others

            service.AddScoped<IPasswordHasher<UserAdmin>, PasswordHasher<UserAdmin>>();

            #endregion
        }
    }
}
