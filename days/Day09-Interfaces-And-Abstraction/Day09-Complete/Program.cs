using ServiceHub.Day09.Data;
using ServiceHub.Day09.Endpoints;
using ServiceHub.Day09.Repositories;
using ServiceHub.Day09.Services;

// Day 09 — DTOs & API Contracts
// Building on Day 08: Adding DTOs, organizing endpoints
// Demonstrates: DTO pattern, organized endpoints, mapper extension methods

var builder = WebApplicationBuilder.CreateBuilder(args);

// ========== DEPENDENCY INJECTION SETUP ==========
// Same as Day 08, but services now use DTOs for responses
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<WorkOrderService>();

// Swagger for documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ========== MIDDLEWARE ==========
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ========== ENDPOINTS (Day 09: Using organized endpoint files + DTOs) ==========

// Health check
app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
    .WithName("Health")
    .WithOpenApi();

// Organize endpoints using extension methods
app.MapCustomerEndpoints();
app.MapWorkOrderEndpoints();

app.Run();
