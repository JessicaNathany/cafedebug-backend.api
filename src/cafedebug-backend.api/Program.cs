using System.Text.Json;
using System.Text.Json.Serialization;
using cafedebug_backend.api.Configurations;
using cafedebug_backend.api.Filters;
using cafedebug_backend.api.Infrastructure.HealthChecks;
using cafedebug_backend.infrastructure.Common.Extensions;
using cafedebug_backend.infrastructure.Constants;
using cafedebug.backend.application.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add default logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add Serilog
builder.Host.AddSerilogConfiguration(builder.Configuration);

// Register Dependencies
builder.Services.ResolveDependencies();

// database configuration
builder.Services.AddDatabaseConfiguration(builder.Configuration, builder.Environment.IsDevelopment());

// get constants
var issuer = JWTConstants.JwtIssuer;
var audience = JWTConstants.JwtAudience;
var secretKey = JWTConstants.JWTSecret;

builder.Services.AddControllers();

//swagger configuration
builder.Services.AddSwaggerConfiguration();

// Add Application layer services (includes validators)
builder.Services.AddApplicationServices();

// Add Infrastructure layer services
builder.Services.AddInfrastructureServices();

// Health Checks
builder.Services.AddHealthChecksConfiguration(builder.Configuration);

// Suppress automatic model state validation
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();