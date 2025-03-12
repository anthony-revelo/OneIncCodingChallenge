using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using UserManagement.Infrastructure.Validation;
using UserManagement.Api.Extensions;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Load configurations from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Configure services
builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Preserve property names as defined
                });

builder.Services.ConfigureServiceDependencies(); // Register application services
builder.Services.ConfigureValidators(); // Register validators

// Registering validators for any specific configuration section that needs to be present for the app to start correctly
builder.Services.AddSingleton<IValidator<MySpecificConfiguration>, MySpecificConfigurationValidator>();
builder.Services.AddOptions<MySpecificConfiguration>()
    .Bind(builder.Configuration.GetSection("MySpecificConfiguration"))
    .ValidateFluently()
    .ValidateOnStart();

builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Load database settings from configuration
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("ConnectionStrings"));

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "User Management API",
        Version = "v1",
        Description = "An API to manage users"
    });
});

// Build the application
var app = builder.Build();

// Validate configurations at startup
var databaseSettings = app.Services.GetRequiredService<IOptionsMonitor<DatabaseSettings>>().CurrentValue;
var validator = new DatabaseSettingsValidator();
var validationResult = validator.Validate(databaseSettings);

if (!validationResult.IsValid)
{
    foreach (var error in validationResult.Errors)
    {
        Console.WriteLine(error.ErrorMessage);
    }
    throw new Exception("Configuration validation failed.");
}

// Enable Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Management API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseRouting();
app.MapControllers(); // Register controllers at the top level

app.Run();
