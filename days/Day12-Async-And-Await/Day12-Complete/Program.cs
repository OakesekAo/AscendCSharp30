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

// ========== ERROR HANDLING MIDDLEWARE ==========
app.Use(async (context, next) =>
{
    try
    {
        await next(context);
    }
    catch (Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        
        var problem = new ProblemDetails
        {
            Title = "An error occurred",
            Detail = ex.Message,
            Status = StatusCodes.Status500InternalServerError,
            Instance = context.Request.Path
        };
        
        await context.Response.WriteAsJsonAsync(problem);
    }
});

// ========== CUSTOMER ENDPOINTS ==========
app.MapGet("/customers", GetCustomersAsync)
.WithName("GetCustomers")
.WithOpenApi()
.Produces<List<CustomerResponse>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status500InternalServerError);

app.MapGet("/customers/{id}", GetCustomerByIdAsync)
.WithName("GetCustomerById")
.WithOpenApi()
.Produces<CustomerResponse>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

app.MapPost("/customers", CreateCustomerAsync)
.WithName("CreateCustomer")
.WithOpenApi()
.Produces<CustomerResponse>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest);

// ========== WORK ORDER ENDPOINTS ==========
app.MapGet("/workorders", GetWorkOrdersAsync)
.WithName("GetWorkOrders")
.WithOpenApi()
.Produces<List<WorkOrderResponse>>(StatusCodes.Status200OK);

app.MapGet("/workorders/{id}", GetWorkOrderByIdAsync)
.WithName("GetWorkOrderById")
.WithOpenApi()
.Produces<WorkOrderResponse>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

app.MapPost("/workorders", CreateWorkOrderAsync)
.WithName("CreateWorkOrder")
.WithOpenApi()
.Produces<WorkOrderResponse>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest);

// ========== FILTERING ENDPOINTS ==========
app.MapGet("/workorders/by-status/{status}", GetWorkOrdersByStatusAsync)
.WithName("GetWorkOrdersByStatus")
.WithOpenApi()
.Produces<List<WorkOrderResponse>>();

app.MapGet("/customers/{id}/workorders", GetCustomerWorkOrdersAsync)
.WithName("GetCustomerWorkOrders")
.WithOpenApi()
.Produces<List<WorkOrderResponse>>();

app.MapGet("/health", HealthCheckAsync)
.WithName("Health")
.WithOpenApi();

app.Run();

// ========== ENDPOINT HANDLERS ==========
async Task<IResult> GetCustomersAsync(CustomerService service)
{
    var customers = await service.ListCustomersAsync();
    return Results.Ok(customers.Select(c => c.ToResponse()).ToList());
}

async Task<IResult> GetCustomerByIdAsync(int id, CustomerService service)
{
    var customer = await service.GetCustomerAsync(id);
    return customer != null ? Results.Ok(customer.ToResponse()) : Results.NotFound();
}

async Task<IResult> CreateCustomerAsync(CreateCustomerRequest request, CustomerService service)
{
    if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email))
        return Results.BadRequest(new ProblemDetails { Title = "Validation Error", Detail = "Name and Email required" });
    
    var customer = request.ToCustomer();
    await service.CreateCustomerAsync(customer);
    return Results.Created($"/customers/{customer.Id}", customer.ToResponse());
}

async Task<IResult> GetWorkOrdersAsync(WorkOrderService service)
{
    var orders = await service.ListWorkOrdersAsync();
    return Results.Ok(orders.Select(o => o.ToResponse()).ToList());
}

async Task<IResult> GetWorkOrderByIdAsync(int id, WorkOrderService service)
{
    var order = await service.GetWorkOrderAsync(id);
    return order != null ? Results.Ok(order.ToResponse()) : Results.NotFound();
}

async Task<IResult> CreateWorkOrderAsync(CreateWorkOrderRequest request, WorkOrderService service)
{
    if (request.CustomerId <= 0 || string.IsNullOrWhiteSpace(request.Description))
        return Results.BadRequest(new ProblemDetails { Title = "Validation Error", Detail = "CustomerId and Description required" });
    
    var order = request.ToWorkOrder();
    await service.CreateWorkOrderAsync(order);
    return Results.Created($"/workorders/{order.Id}", order.ToResponse());
}

async Task<IResult> GetWorkOrdersByStatusAsync(string status, WorkOrderService service)
{
    var orders = await service.GetWorkOrdersByStatusAsync(status);
    return Results.Ok(orders.Select(o => o.ToResponse()).ToList());
}

async Task<IResult> GetCustomerWorkOrdersAsync(int id, CustomerService service, WorkOrderService workOrderService)
{
    var customer = await service.GetCustomerAsync(id);
    if (customer == null) return Results.NotFound();
    
    var orders = await workOrderService.GetWorkOrdersForCustomerAsync(id);
    return Results.Ok(orders.Select(o => o.ToResponse()).ToList());
}

async Task<IResult> HealthCheckAsync()
{
    await Task.Delay(10);
    return Results.Ok(new 
    { 
        status = "ServiceHub API v0.4 - Error Handling", 
        timestamp = DateTime.UtcNow,
        version = "0.4.0"
    });
}

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
    Task<List<WorkOrder>> GetByStatusAsync(string status);
    Task<List<WorkOrder>> GetByCustomerAsync(int customerId);
}

// ========== REPOSITORIES ==========
class CustomerRepository : ICustomerRepository
{
    private List<Customer> customers = new();
    private int nextId = 1;
    
    public async Task AddCustomerAsync(Customer customer)
    {
        await Task.Delay(10);
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
    
    public async Task<List<WorkOrder>> GetByStatusAsync(string status)
    {
        await Task.Delay(10);
        return orders.Where(o => o.Status == status).ToList();
    }
    
    public async Task<List<WorkOrder>> GetByCustomerAsync(int customerId)
    {
        await Task.Delay(10);
        return orders.Where(o => o.CustomerId == customerId).ToList();
    }
}

// ========== SERVICES ==========
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
    
    public async Task<List<WorkOrder>> GetWorkOrdersByStatusAsync(string status) =>
        await repository.GetByStatusAsync(status);
    
    public async Task<List<WorkOrder>> GetWorkOrdersForCustomerAsync(int customerId) =>
        await repository.GetByCustomerAsync(customerId);
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

record ProblemDetails(string Title, string Detail, int Status, string Instance);

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
