using ServiceHub.Day11.Data;
using ServiceHub.Day11.Endpoints;
using ServiceHub.Day11.Repositories;
using ServiceHub.Day11.Services;

// Day 11 — Search & Filtering
// Building on Day 10: Adding search and filtering capabilities
// Demonstrates: Repository search methods, query filtering, LINQ operations

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<WorkOrderService>();

// Use built-in .NET 10 API UI (Scaler) instead of Swashbuckle/Swagger
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Scaler UI will be used by the runtime in .NET 10; no Swashbuckle middleware needed here.
}

app.UseHttpsRedirection();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
    .WithName("Health")
    .WithOpenApi();

app.MapCustomerEndpoints();
app.MapWorkOrderEndpoints();

app.Run();
