using ServiceHub.Day13.Data;
using ServiceHub.Day13.Endpoints;
using ServiceHub.Day13.Repositories;
using ServiceHub.Day13.Services;

// Day 13 — Advanced Features (Pagination & Sorting)
// Building on Day 12: Adding pagination and sorting capabilities
// Demonstrates: Pagination logic, sorting, advanced queries, LINQ Skip/Take

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
    // Scaler UI will be used by the runtime in .NET 10; remove Swashbuckle calls.
}

app.UseHttpsRedirection();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
    .WithName("Health")
    .WithOpenApi();

app.MapCustomerEndpoints();
app.MapWorkOrderEndpoints();

app.Run();
