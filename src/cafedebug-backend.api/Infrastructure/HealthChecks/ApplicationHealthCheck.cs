using System.Diagnostics;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace cafedebug_backend.api.Infrastructure.HealthChecks;

public class ApplicationHealthCheck(ILogger<ApplicationHealthCheck> logger) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Check memory usage
            var process = Process.GetCurrentProcess();
            var memoryUsed = process.WorkingSet64 / 1024 / 1024; // MB
            var cpuTime = process.TotalProcessorTime;

            // Define thresholds
            const long warningMemoryThreshold = 500; // MB
            const long unhealthyMemoryThreshold = 1000; // MB

            var data = new Dictionary<string, object>
            {
                { "MemoryUsedMB", memoryUsed },
                { "CPUTime", cpuTime.ToString() },
                { "ThreadCount", process.Threads.Count },
                { "HandleCount", process.HandleCount }
            };

            if (memoryUsed > unhealthyMemoryThreshold)
            {
                logger.LogWarning("Application memory usage is high: {MemoryMB} MB", memoryUsed);
                return Task.FromResult(
                    HealthCheckResult.Unhealthy(
                        $"Memory usage is too high: {memoryUsed} MB",
                        data: data));
            }

            if (memoryUsed > warningMemoryThreshold)
            {
                logger.LogWarning("Application memory usage is elevated: {MemoryMB} MB", memoryUsed);
                return Task.FromResult(
                    HealthCheckResult.Degraded(
                        $"Memory usage is elevated: {memoryUsed} MB",
                        data: data));
            }

            logger.LogDebug("Application health check successful");
            
            return Task.FromResult(
                HealthCheckResult.Healthy(
                    "Application is running normally",
                    data: data));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Application health check failed");
            
            return Task.FromResult(
                HealthCheckResult.Unhealthy(
                    "Application health check failed",
                    exception: ex));
        }
    }
}