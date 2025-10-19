using System.Text.Json;
using cafedebug_backend.api.Configurations;
using cafedebug_backend.api.DependencyInjection;
using cafedebug_backend.api.Filters;
using cafedebug_backend.api.Infrastructure.HealthChecks;
using cafedebug_backend.application.Constants;
using cafedebug_backend.infrastructure.Context;
using cafedebug.backend.application.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

//swagger configuration
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Application layer services (includes validators)
builder.Services.AddApplicationServices();

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
    });

var app = builder.Build();

// Add Serilog request logging
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
        diagnosticContext.Set("RemoteIP", httpContext.Connection.RemoteIpAddress?.ToString());
        diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent.ToString());
    };
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAppHealthChecks();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();