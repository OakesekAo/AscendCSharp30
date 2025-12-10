using ServiceHub.Day10.Data;
using ServiceHub.Day10.Endpoints;
using ServiceHub.Day10.Repositories;
using ServiceHub.Day10.Services;

// Day 10 — Error Handling & Validation
// Building on Day 09: Adding professional error responses and validation
// Demonstrates: Error DTO, validation methods, try-catch patterns, status codes

var builder = WebApplicationBuilder.CreateBuilder(args);

// ========== DEPENDENCY INJECTION SETUP ==========
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

// ========== ENDPOINTS (Day 10: With error handling & validation) ==========

// Health check
app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
    .WithName("Health")
    .WithOpenApi();

// Organize endpoints with error handling
app.MapCustomerEndpoints();
app.MapWorkOrderEndpoints();

app.Run();
