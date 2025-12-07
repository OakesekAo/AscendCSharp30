using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

var builder = WebApplicationBuilder.CreateBuilder(args);

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<WorkOrderService>();

var app = builder.Build();

app.UseHttpsRedirection();

// ========== ASYNC CUSTOMER ENDPOINTS ==========
app.MapGet("/customers", async (CustomerService service) =>
{
    // Simulate async work
    var customers = await service.ListCustomersAsync();
    var responses = customers.Select(c => c.ToResponse()).ToList();
    return Results.Ok(responses);
})
.WithName("GetCustomers")
.WithOpenApi();

app.MapGet("/customers/{id}", async (int id, CustomerService service) =>
{
    var customer = await service.GetCustomerAsync(id);
    return customer != null 
        ? Results.Ok(customer.ToResponse()) 
        : Results.NotFound();
})
.WithName("GetCustomerById")
.WithOpenApi();

app.MapPost("/customers", async (CreateCustomerRequest request, CustomerService service) =>
{
    // Validate input
    if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email))
    {
        return Results.BadRequest("Name and Email are required");
    }
    
    var customer = request.ToCustomer();
    await service.CreateCustomerAsync(customer);
    return Results.Created($"/customers/{customer.Id}", customer.ToResponse());
})
.WithName("CreateCustomer")
.WithOpenApi();

// ========== ASYNC WORK ORDER ENDPOINTS ==========
app.MapGet("/workorders", async (WorkOrderService service) =>
{
    var orders = await service.ListWorkOrdersAsync();
    var responses = orders.Select(o => o.ToResponse()).ToList();
    return Results.Ok(responses);
})
.WithName("GetWorkOrders")
.WithOpenApi();

app.MapGet("/workorders/{id}", async (int id, WorkOrderService service) =>
{
    var order = await service.GetWorkOrderAsync(id);
    return order != null 
        ? Results.Ok(order.ToResponse()) 
        : Results.NotFound();
})
.WithName("GetWorkOrderById")
.WithOpenApi();

app.MapPost("/workorders", async (CreateWorkOrderRequest request, WorkOrderService service) =>
{
    if (request.CustomerId <= 0 || string.IsNullOrWhiteSpace(request.Description))
    {
        return Results.BadRequest("CustomerId and Description are required");
    }
    
    var order = request.ToWorkOrder();
    await service.CreateWorkOrderAsync(order);
    return Results.Created($"/workorders/{order.Id}", order.ToResponse());
})
.WithName("CreateWorkOrder")
.WithOpenApi();

app.MapGet("/health", async () =>
{
    // Simulate async health check
    await Task.Delay(10);
    return Results.Ok(new { status = "ServiceHub API v0.3", timestamp = DateTime.UtcNow });
})
.WithName("Health")
.WithOpenApi();

app.Run();

// ========== INTERFACES ==========
interface ICustomerRepository
{
    Task AddCustomerAsync(Customer customer);
    Task<Customer?> GetCustomerAsync(int id);
    Task<List<Customer>> GetAllAsync();
}

interface IWorkOrderRepository
{
    Task AddWorkOrderAsync(WorkOrder order);
    Task<WorkOrder?> GetWorkOrderAsync(int id);
    Task<List<WorkOrder>> GetAllAsync();
}

// ========== ASYNC REPOSITORIES ==========
class CustomerRepository : ICustomerRepository
{
    private List<Customer> customers = new();
    private int nextId = 1;
    
    public async Task AddCustomerAsync(Customer customer)
    {
        await Task.Delay(10); // Simulate DB call
        customer.Id = nextId++;
        customers.Add(customer);
    }
    
    public async Task<Customer?> GetCustomerAsync(int id)
    {
        await Task.Delay(10);
        return customers.FirstOrDefault(c => c.Id == id);
    }
    
    public async Task<List<Customer>> GetAllAsync()
    {
        await Task.Delay(10);
        return customers;
    }
}

class WorkOrderRepository : IWorkOrderRepository
{
    private List<WorkOrder> orders = new();
    private int nextId = 1;
    
    public async Task AddWorkOrderAsync(WorkOrder order)
    {
        await Task.Delay(10);
        order.Id = nextId++;
        orders.Add(order);
    }
    
    public async Task<WorkOrder?> GetWorkOrderAsync(int id)
    {
        await Task.Delay(10);
        return orders.FirstOrDefault(o => o.Id == id);
    }
    
    public async Task<List<WorkOrder>> GetAllAsync()
    {
        await Task.Delay(10);
        return orders;
    }
}

// ========== ASYNC SERVICES ==========
class CustomerService
{
    private ICustomerRepository repository;
    public CustomerService(ICustomerRepository repo) => repository = repo;
    
    public async Task CreateCustomerAsync(Customer customer) =>
        await repository.AddCustomerAsync(customer);
    
    public async Task<Customer?> GetCustomerAsync(int id) =>
        await repository.GetCustomerAsync(id);
    
    public async Task<List<Customer>> ListCustomersAsync() =>
        await repository.GetAllAsync();
}

class WorkOrderService
{
    private IWorkOrderRepository repository;
    public WorkOrderService(IWorkOrderRepository repo) => repository = repo;
    
    public async Task CreateWorkOrderAsync(WorkOrder order) =>
        await repository.AddWorkOrderAsync(order);
    
    public async Task<WorkOrder?> GetWorkOrderAsync(int id) =>
        await repository.GetWorkOrderAsync(id);
    
    public async Task<List<WorkOrder>> ListWorkOrdersAsync() =>
        await repository.GetAllAsync();
}

// ========== DOMAIN MODELS ==========
class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
}

class WorkOrder
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string Description { get; set; } = "";
    public string Status { get; set; } = "Scheduled";
}

// ========== DTOs ==========
record CreateCustomerRequest(string Name, string Email);
record CustomerResponse(int Id, string Name, string Email);

record CreateWorkOrderRequest(int CustomerId, string Description, string Status);
record WorkOrderResponse(int Id, int CustomerId, string Description, string Status);

// ========== MAPPERS ==========
static class Mappers
{
    public static Customer ToCustomer(this CreateCustomerRequest request) =>
        new() { Name = request.Name, Email = request.Email };
    
    public static CustomerResponse ToResponse(this Customer customer) =>
        new(customer.Id, customer.Name, customer.Email);
    
    public static WorkOrder ToWorkOrder(this CreateWorkOrderRequest request) =>
        new() { CustomerId = request.CustomerId, Description = request.Description, Status = request.Status };
    
    public static WorkOrderResponse ToResponse(this WorkOrder order) =>
        new(order.Id, order.CustomerId, order.Description, order.Status);
}
