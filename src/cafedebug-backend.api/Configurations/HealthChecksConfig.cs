using cafedebug_backend.api.Infrastructure.HealthChecks;
using cafedebug_backend.infrastructure.Context;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace cafedebug_backend.api.Configurations;

public static class HealthChecksConfig
{
    public static IServiceCollection AddHealthChecksConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        
        var connectionString = configuration.GetConnectionString("CafedebugConnectionStringMySQL");

        services
            .AddHealthChecksUI(setup =>
            {
                setup.MaximumHistoryEntriesPerEndpoint(50);
                setup.AddHealthCheckEndpoint("Cafe Debug API Health Check", "/health");
            })
            .AddInMemoryStorage();
        
        services.AddHealthChecks()
            .AddCheck<ApplicationHealthCheck>(
                "Application",
                failureStatus: HealthStatus.Unhealthy,
                tags: ["application", "ready"])
            .AddCheck<DatabaseHealthCheck>(
                "Database",
                failureStatus: HealthStatus.Unhealthy,
                tags: ["database", "ready"])
            .AddDbContextCheck<CafedebugContext>(
                "CafedebugContext",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "database", "efcore" })
            .AddMySql(
                connectionString,
                name: "MySQL Connection",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "database", "mysql" });

        return services;
    }
}