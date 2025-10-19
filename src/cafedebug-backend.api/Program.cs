using System.Text.Json;
using cafedebug_backend.api.DependencyInjection;
using cafedebug_backend.api.Filters;
using cafedebug_backend.application.Constants;
using cafedebug_backend.infrastructure.Context;
using cafedebug.backend.application.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configura��es de logging
// builder.Logging.ClearProviders();
// builder.Logging.AddConsole();
// builder.Logging.AddDebug();
// builder.Logging.AddEventSourceLogger();
// builder.Logging.AddEventLog(settings =>
// {
//     settings.LogName = "Application";
//     settings.SourceName = "Cafedebug";
// });

// Register Depndencies
builder.Services.ResolveDependencies();

// configure Dbcontext class
builder.Services.AddDbContextPool<CafedebugContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("CafedebugConnectionStringMySQL");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), options =>
        options.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null));
});

// get constants
var issuer = JWTConstants.JwtIssuer;
var audience = JWTConstants.JwtAudience;
var secretKey = JWTConstants.JWTSecret;

//swagger configuration
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Loggin Configuration
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// Add Application layer services (includes validators)
builder.Services.AddApplicationServices();

// Suppress automatic model state validation
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Add FluentValidation to ASP.NET Core
builder.Services.AddControllers(options =>
    {
        options.Filters.Add<AfterHandlerActionFilterAttribute>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();