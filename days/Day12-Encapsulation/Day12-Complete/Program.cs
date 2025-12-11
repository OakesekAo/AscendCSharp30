using ServiceHub.Day12.Data;
using ServiceHub.Day12.Endpoints;
using ServiceHub.Day12.Repositories;
using ServiceHub.Day12.Services;

// Day 12 — Analytics & Reporting
// Building on Day 11: Adding analytics and statistics endpoints
// Demonstrates: Aggregation, statistics calculation, advanced queries

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<WorkOrderService>();
builder.Services.AddScoped<AnalyticsService>();

// Use built-in .NET 10 API UI (Scaler) instead of Swashbuckle/Swagger
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Use the built-in .NET 10 API UI (Scaler). Swashbuckle middleware removed to avoid dependency conflicts.
}

app.UseHttpsRedirection();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
    .WithName("Health")
    .WithOpenApi();

app.MapCustomerEndpoints();
app.MapWorkOrderEndpoints();
app.MapAnalyticsEndpoints();

app.Run();
