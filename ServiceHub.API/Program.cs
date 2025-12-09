using ServiceHub.API.Endpoints;
using ServiceHub.API.Extensions;

var builder = WebApplicationBuilder.CreateBuilder(args);

// ========== DEPENDENCY INJECTION ==========
builder.Services.AddServiceHubServices();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ServiceHub API",
        Version = "1.0.0",
        Description = "Professional REST API for ServiceHub - Week 2 of AscendCSharp30"
    });
});

var app = builder.Build();

// ========== MIDDLEWARE ==========
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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
