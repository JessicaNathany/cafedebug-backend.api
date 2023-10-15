using cafedebug.backend.application.Service;
using cafedebug_backend.domain.Interfaces.JWT;
using cafedebug_backend.domain.Interfaces.Respositories;
using cafedebug_backend.infrastructure.Data.Repository;

namespace cafedebug_backend.api.DependencyInjection
{
    public static class DependencyInjectionRegistry
    {
        public static void ResolveDependencies(this IServiceCollection service)
        {
            service.AddScoped<IBannerRepository, BannerRepository>();
            service.AddScoped<IEpisodeRepository, EpisodeRepository>();
            service.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            service.AddScoped<IJWTService, JWTService>();
        }
    }
}
