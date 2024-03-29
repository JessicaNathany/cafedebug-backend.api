using cafedebug_backend.api.DependencyInjection;
using cafedebug_backend.application.Constants;
using cafedebug_backend.infrastructure.Context;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Register Depndencies
builder.Services.ResolveDependencies();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//// add localization service
//builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

//// configure support culture 
//var supportedCultures = new[] { "en-US", "pt-BR" };

//var localizationOptions = new RequestLocalizationOptions()
//    .SetDefaultCulture(supportedCultures[0])
//    .AddSupportedCultures(supportedCultures)
//    .AddSupportedUICultures(supportedCultures);


//builder.Services.Configure<RequestLocalizationOptions>(options =>
//{
//    options.DefaultRequestCulture = new RequestCulture(supportedCultures[0]);
//    options.SupportedCultures = localizationOptions.SupportedCultures;
//    options.SupportedUICultures = localizationOptions.SupportedUICultures;
//});


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

// Add services authentication JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        //is the key used to validate the token's signature.
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
        ClockSkew = TimeSpan.Zero,
    };
});

builder.Services.AddAuthorization();

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

// Use localization Resx
//app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Use authentication and authorization.
app.UseAuthentication();
app.UseAuthorization();

app.Run();
