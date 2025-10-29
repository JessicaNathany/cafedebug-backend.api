using Microsoft.OpenApi.Models;

namespace cafedebug_backend.api.Configurations;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(s =>
        {
            s.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Café Debug API",
                Version = "v1",
                Description = "API for CafeDebug",
                Contact = new OpenApiContact { Name = "CafeDebug", Url = new Uri("https://cafedebug.example") },
                License = new OpenApiLicense { Name = "MIT" }
            });

            s.DescribeAllParametersInCamelCase();

            s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Input the JWT like: Bearer {your token}",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            s.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            });
        });

        return services;
    }

    public static void UseSwaggerSetup(this IApplicationBuilder app)
    {
        if (app == null) throw new ArgumentNullException(nameof(app));

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "CafeDebug API v1");
            c.DocumentTitle = "CafeDebug API Docs";
            c.DisplayRequestDuration();
            c.DefaultModelsExpandDepth(-1); // hide schema section by default (less noise)
            c.EnableFilter(); 
        });
    }
}