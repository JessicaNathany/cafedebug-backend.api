using cafedebug_backend.api.DependencyInjection;
using cafedebug_backend.application.Constants;
using cafedebug_backend.infrastructure.Context;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configurações de logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventSourceLogger();
builder.Logging.AddEventLog(settings =>
{
    settings.LogName = "Application";
    settings.SourceName = "Cafedebug";
});

// Register Depndencies
builder.Services.ResolveDependencies();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// configure Dbcontext class
builder.Services.AddDbContextPool<CafedebugContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("CafedebugConnectionStringMySQL");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), options =>
    options.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null));
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

// FluentValidation
builder.Services.AddControllers().AddFluentValidation(fluentValidation =>
{
    fluentValidation.RegisterValidatorsFromAssemblyContaining<Program>();
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