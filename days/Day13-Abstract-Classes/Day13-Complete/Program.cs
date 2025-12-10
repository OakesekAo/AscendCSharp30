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

app.Run();
