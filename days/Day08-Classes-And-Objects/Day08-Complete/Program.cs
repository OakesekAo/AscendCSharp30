using ServiceHub.Day08.Data;
using ServiceHub.Day08.Repositories;
using ServiceHub.Day08.Services;

// Day 08 — Dependency Injection Foundations
// Building the ServiceHub API starting with DI + basic endpoints
// Demonstrates: Constructor injection, interfaces, DI container, basic endpoints

var builder = WebApplicationBuilder.CreateBuilder(args);

// ========== DEPENDENCY INJECTION SETUP ==========
// This is the core of Day 08: Learning to register and inject dependencies
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

// ========== ENDPOINTS (Day 08: Basic CRUD with DI) ==========

// Health check
app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
    .WithName("Health")
    .WithOpenApi();

// Customers
app.MapGet("/customers", async (CustomerService service) =>
{
    var customers = await service.GetAllAsync();
    return Results.Ok(customers);
})
.WithName("GetCustomers")
.WithOpenApi();

app.MapGet("/customers/{id}", async (int id, CustomerService service) =>
{
    var customer = await service.GetAsync(id);
    return customer != null ? Results.Ok(customer) : Results.NotFound();
})
.WithName("GetCustomerById")
.WithOpenApi();

app.MapPost("/customers", async (CreateCustomerRequest request, CustomerService service) =>
{
    if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email))
        return Results.BadRequest("Name and Email required");

    var customer = await service.CreateAsync(request.Name, request.Email);
    return Results.Created($"/customers/{customer.Id}", customer);
})
.WithName("CreateCustomer")
.WithOpenApi();

// Work Orders
app.MapGet("/workorders", async (WorkOrderService service) =>
{
    var orders = await service.GetAllAsync();
    return Results.Ok(orders);
})
.WithName("GetWorkOrders")
.WithOpenApi();

app.MapGet("/workorders/{id}", async (int id, WorkOrderService service) =>
{
    var order = await service.GetAsync(id);
    return order != null ? Results.Ok(order) : Results.NotFound();
})
.WithName("GetWorkOrderById")
.WithOpenApi();

app.MapPost("/workorders", async (CreateWorkOrderRequest request, WorkOrderService service) =>
{
    if (request.CustomerId <= 0 || string.IsNullOrWhiteSpace(request.Description))
        return Results.BadRequest("CustomerId and Description required");

    var order = await service.CreateAsync(request.CustomerId, request.Description);
    return Results.Created($"/workorders/{order.Id}", order);
})
.WithName("CreateWorkOrder")
.WithOpenApi();

app.Run();

// ========== DTOs ==========
record CreateCustomerRequest(string Name, string Email);
record CreateWorkOrderRequest(int CustomerId, string Description);
