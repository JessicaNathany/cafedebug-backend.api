using System.Reflection;
using AutoMapper;
using cafedebug.backend.application.Common.Validations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace cafedebug.backend.application.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // register from the assembly marker
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Add SharpGrip FluentValidation auto validation
        services.AddFluentValidationAutoValidation(configuration =>
        {
            // Disable built-in model validation to avoid duplicates
            configuration.DisableBuiltInModelValidation = true;
            configuration.OverrideDefaultResultFactoryWith<AutoValidationResultFactory>();
        });

        // Register application services
        services.Scan(scan => scan
            .FromAssemblies(Assembly.GetExecutingAssembly()) // application assembly
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // AutoMapper
        services.AddSingleton<IMapper>(provider =>
        {
            var loggerFactory = provider.GetService<ILoggerFactory>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(Assembly.GetExecutingAssembly());
                cfg.ConstructServicesUsing(provider.GetService);
            }, loggerFactory);

            return config.CreateMapper();
        });

        return services;
    }
}