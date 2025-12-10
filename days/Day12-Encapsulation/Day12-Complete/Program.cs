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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
    .WithName("Health")
    .WithOpenApi();

app.MapCustomerEndpoints();
app.MapWorkOrderEndpoints();
app.MapAnalyticsEndpoints();

app.Run();
