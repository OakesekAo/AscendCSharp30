using ServiceHub.API.Endpoints;
using ServiceHub.API.Extensions;

var builder = WebApplicationBuilder.CreateBuilder(args);

// ========== DEPENDENCY INJECTION ==========
builder.Services.AddServiceHubServices();

// Use built-in .NET 10 API UI (Scaler). If Swashbuckle needed, add it explicitly per-project.
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// ========== MIDDLEWARE ==========
// Use built-in .NET 10 API UI (Scaler) in development — no Swashbuckle middleware required

app.UseHttpsRedirection();

// ========== SYSTEM ENDPOINTS ==========
app.MapGet("/", () => Results.Ok(new
{
    name = "ServiceHub API",
    version = "1.0.0",
    status = "running",
    timestamp = DateTime.UtcNow,
    endpoints = new[] { "/customers", "/workorders", "/analytics", "/health" }
}))
.WithName("Root")
.WithOpenApi();

app.MapGet("/health", () => Results.Ok(new { status = "ok", timestamp = DateTime.UtcNow }))
.WithName("Health")
.WithOpenApi();

// ========== ROUTE MAPPINGS ==========
app.MapCustomerEndpoints();
app.MapWorkOrderEndpoints();
app.MapAnalyticsEndpoints();

app.Run();
