using System.Text.Json;
using System.Text.Json.Serialization;
using cafedebug_backend.api.Configurations;
using cafedebug_backend.api.Filters;
using cafedebug_backend.api.Infrastructure.HealthChecks;
using cafedebug_backend.api.Middleware;
using cafedebug_backend.infrastructure.Common.Extensions;
using cafedebug.backend.application.Common.Extensions;
using DotNetEnv;
using Microsoft.AspNetCore.Mvc;

// Load the .env file
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add default logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add Serilog
builder.Host.AddSerilogConfiguration(builder.Configuration);

// Register Dependencies service and jwt
builder.Services.ResolveDependencies(builder.Configuration);

// database configuration
builder.Services.AddDatabaseConfiguration(builder.Configuration, builder.Environment.IsDevelopment());

builder.Services.AddControllers();

//swagger configuration
builder.Services.AddSwaggerConfiguration();

// Add Application layer services (includes validators)
builder.Services.AddApplicationServices();

//Add Infrastructure layer services
builder.Services.AddInfrastructureServices();

// Health Checks
builder.Services.AddHealthChecksConfiguration(builder.Configuration);

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Add FluentValidation to ASP.NET Core
builder.Services.AddControllers(options =>
{
    options.Filters.Add<AfterHandlerActionFilterAttribute>();
    options.Filters.Add<ApiExceptionFilterAttribute>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerSetup();
}

app.UseSerilog();
app.UseAppHealthChecks();
app.UseHttpsRedirection();

// Add authentication middleware before UseAuthentication
app.UseMiddleware<AuthenticationMiddleware>();

app.UseAuthentication();  
app.UseAuthorization();   

app.MapControllers();
app.Run();