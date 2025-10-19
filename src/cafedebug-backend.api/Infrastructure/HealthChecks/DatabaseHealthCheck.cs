using cafedebug_backend.infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace cafedebug_backend.api.Infrastructure.HealthChecks;

public class DatabaseHealthCheck(CafedebugContext context, ILogger<DatabaseHealthCheck> logger)
    : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context1,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if a database can be connected
            var canConnect = await context.Database.CanConnectAsync(cancellationToken);

            if (!canConnect)
            {
                logger.LogWarning("Database connection failed");
                return HealthCheckResult.Unhealthy(
                    "Database connection failed",
                    exception: null,
                    data: new Dictionary<string, object>
                    {
                        { "ConnectionString", context.Database.GetConnectionString() ?? "Not configured" }
                    });
            }

            // Execute a simple query to verify a database is responsive
            var episodeCount = await context.Episodes.CountAsync(cancellationToken);

            logger.LogDebug("Database health check successful. Episodes count: {Count}", episodeCount);

            return HealthCheckResult.Healthy(
                "Database is healthy",
                data: new Dictionary<string, object>
                {
                    { "EpisodeCount", episodeCount },
                    { "Database", context.Database.GetDbConnection().Database }
                });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database health check failed");

            return HealthCheckResult.Unhealthy(
                "Database health check failed",
                exception: ex,
                data: new Dictionary<string, object>
                {
                    { "Error", ex.Message }
                });
        }
    }
}