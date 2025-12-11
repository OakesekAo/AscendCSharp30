using ServiceHub.Day14.Data;
using ServiceHub.Day14.Endpoints;
using ServiceHub.Day14.Repositories;
using ServiceHub.Day14.Services;

// Day 14 — Production Ready (Logging, Configuration, Middleware)
// Building on Day 13: Adding production-grade features
// Demonstrates: Logging, configuration management, global error handling, API versioning

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<WorkOrderService>();
builder.Services.AddScoped<LoggingService>();

// Use built-in .NET 10 API UI (Scaler) instead of Swashbuckle/Swagger
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Global error handling middleware
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError($"Unhandled exception: {ex.Message}");
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new { error = "Internal server error", message = ex.Message });
    }
});

if (app.Environment.IsDevelopment())
{
    // Scaler UI will be used by the runtime in .NET 10; no Swashbuckle middleware here.
}

app.UseHttpsRedirection();

app.MapGet("/health", () => Results.Ok(new { status = "ok", timestamp = DateTime.UtcNow }))
    .WithName("Health")
    .WithOpenApi();

app.MapGet("/info", () => Results.Ok(new { 
    version = "1.0.0", 
    environment = app.Environment.EnvironmentName,
    timestamp = DateTime.UtcNow
}))
    .WithName("Info")
    .WithOpenApi();

app.MapCustomerEndpoints();
app.MapWorkOrderEndpoints();

app.Run();
