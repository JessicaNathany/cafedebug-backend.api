using System.Reflection;
using Amazon;
using Amazon.S3;
using cafedebug_backend.domain.Media.Services;
using cafedebug_backend.infrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace cafedebug_backend.infrastructure.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        // Register infrastructure services
        services.Scan(scan => scan
            .FromAssemblies(Assembly.GetExecutingAssembly()) // application assembly
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        // Register infrastructure repositories
        services.Scan(scan => scan
            .FromAssemblies(Assembly.GetExecutingAssembly()) // application assembly
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        #region AWS S3 Configuration
        services.AddOptions<StorageSettings>()
            .BindConfiguration("Storage:AWS:S3") 
            .ValidateOnStart();
        
        services.AddSingleton(sp => 
            sp.GetRequiredService<IOptions<StorageSettings>>().Value);

        // Register IAmazonS3 with configuration
        services.AddSingleton<IAmazonS3>(sp =>
        {
            var settings = sp.GetRequiredService<StorageSettings>();
            
            // For MinIO/LocalStack (custom endpoint)
            if (!string.IsNullOrWhiteSpace(settings.ServiceUrl))
            {
                var config = new AmazonS3Config
                {
                    ServiceURL = settings.ServiceUrl,
                    ForcePathStyle = settings.ForcePathStyle,
                    UseHttp = settings.UseHttp
                };
                
                return new AmazonS3Client(config);
            }
            
            // For AWS S3 
            var awsConfig = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(settings.Region),
                ForcePathStyle = settings.ForcePathStyle
            };
            
            return new AmazonS3Client(awsConfig);
        });
        #endregion
    }
}