using cafedebug_backend.infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.api.Configurations;

public static class DatabaseConfig
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services,
        IConfiguration configuration, bool isDevelopment)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        
        var connectionString = configuration.GetConnectionString("CafedebugConnectionStringMySQL");

        services.AddDbContext<CafedebugContext>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), options =>
            {
                options.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorNumbersToAdd: null);
                options.CommandTimeout(10);
            });

            if (!isDevelopment) return;
            
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        });

        return services;
    }
}