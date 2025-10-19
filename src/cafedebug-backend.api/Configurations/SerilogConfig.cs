using Serilog;

namespace cafedebug_backend.api.Configurations;

public static class SerilogConfig
{
    public static IHostBuilder AddSerilogConfiguration(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        hostBuilder.UseSerilog((context, configureLogger) => configureLogger
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext());  
        return hostBuilder;
    }
}