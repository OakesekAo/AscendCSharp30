using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

var builder = WebApplicationBuilder.CreateBuilder(args);

// ========== DI SETUP ==========
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<WorkOrderService>();
builder.Services.AddScoped<AnalyticsService>();

var app = builder.Build();
app.UseHttpsRedirection();

// ========== ERROR HANDLING ==========
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
        await context.Response.WriteAsJsonAsync(new { error = ex.Message, type = ex.GetType().Name });
    }
});

// ========== CUSTOMER ROUTES ==========
app.MapGet("/customers", GetCustomersAsync).WithOpenApi();
app.MapGet("/customers/{id}", GetCustomerAsync).WithOpenApi();
app.MapPost("/customers", CreateCustomerAsync).WithOpenApi();

// ========== WORK ORDER ROUTES ==========
app.MapGet("/workorders", GetWorkOrdersAsync).WithOpenApi();
app.MapGet("/workorders/{id}", GetWorkOrderAsync).WithOpenApi();
app.MapPost("/workorders", CreateWorkOrderAsync).WithOpenApi();
app.MapPut("/workorders/{id}/status", UpdateWorkOrderStatusAsync).WithOpenApi();

// ========== FILTER & SEARCH ==========\napp.MapGet(\"/workorders/status/{status}\", GetWorkOrdersByStatusAsync).WithOpenApi();
app.MapGet(\"/customers/{id}/workorders\", GetCustomerWorkOrdersAsync).WithOpenApi();
app.MapGet(\"/workorders/search/{term}\", SearchWorkOrdersAsync).WithOpenApi();

// ========== ANALYTICS ==========\napp.MapGet(\"/analytics/summary\", GetAnalyticsSummaryAsync).WithOpenApi();
app.MapGet(\"/analytics/by-status\", GetAnalyticsByStatusAsync).WithOpenApi();
app.MapGet(\"/analytics/customer/{id}\", GetCustomerAnalyticsAsync).WithOpenApi();

// ========== HEALTH & INFO ==========
app.MapGet(\"/health\", HealthCheckAsync).WithOpenApi();
app.MapGet(\"/info\", InfoAsync).WithOpenApi();

app.Run();

// ========== HANDLERS ==========
async Task<IResult> GetCustomersAsync(CustomerService service)
{
    var customers = await service.ListCustomersAsync();
    return Results.Ok(new { total = customers.Count, customers = customers.Select(c => c.ToResponse()).ToList() });
}

async Task<IResult> GetCustomerAsync(int id, CustomerService service)
{
    var customer = await service.GetCustomerAsync(id);
    return customer != null ? Results.Ok(customer.ToResponse()) : Results.NotFound(new { error = \"Customer not found\" });
}

async Task<IResult> CreateCustomerAsync(CreateCustomerRequest request, CustomerService service)
{
    if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email))
        return Results.BadRequest(new { error = \"Name and Email are required\" });
    
    var customer = request.ToCustomer();
    await service.CreateCustomerAsync(customer);
    return Results.Created($\"/customers/{customer.Id}\", customer.ToResponse());
}

async Task<IResult> GetWorkOrdersAsync(WorkOrderService service)
{
    var orders = await service.ListWorkOrdersAsync();
    return Results.Ok(new { total = orders.Count, workorders = orders.Select(o => o.ToResponse()).ToList() });
}

async Task<IResult> GetWorkOrderAsync(int id, WorkOrderService service)
{
    var order = await service.GetWorkOrderAsync(id);
    return order != null ? Results.Ok(order.ToResponse()) : Results.NotFound(new { error = \"Work order not found\" });
}

async Task<IResult> CreateWorkOrderAsync(CreateWorkOrderRequest request, WorkOrderService service)
{
    if (request.CustomerId <= 0 || string.IsNullOrWhiteSpace(request.Description))
        return Results.BadRequest(new { error = \"CustomerId and Description required\" });
    
    var order = request.ToWorkOrder();
    await service.CreateWorkOrderAsync(order);
    return Results.Created($\"/workorders/{order.Id}\", order.ToResponse());
}

async Task<IResult> UpdateWorkOrderStatusAsync(int id, UpdateWorkOrderStatusRequest request, WorkOrderService service)
{
    var order = await service.GetWorkOrderAsync(id);
    if (order == null) return Results.NotFound();
    
    order.Status = request.Status;
    await service.UpdateWorkOrderAsync(order);
    return Results.Ok(order.ToResponse());
}

async Task<IResult> GetWorkOrdersByStatusAsync(string status, WorkOrderService service)
{
    var orders = await service.GetWorkOrdersByStatusAsync(status);
    return Results.Ok(new { status, total = orders.Count, workorders = orders.Select(o => o.ToResponse()).ToList() });
}

async Task<IResult> GetCustomerWorkOrdersAsync(int id, CustomerService service, WorkOrderService workOrderService)
{
    var customer = await service.GetCustomerAsync(id);
    if (customer == null) return Results.NotFound();
    
    var orders = await workOrderService.GetWorkOrdersForCustomerAsync(id);
    return Results.Ok(new { customer = customer.ToResponse(), workorders = orders.Select(o => o.ToResponse()).ToList() });
}

async Task<IResult> SearchWorkOrdersAsync(string term, WorkOrderService service)
{
    var orders = await service.SearchWorkOrdersAsync(term);
    return Results.Ok(new { search_term = term, results = orders.Select(o => o.ToResponse()).ToList() });
}

async Task<IResult> GetAnalyticsSummaryAsync(AnalyticsService analytics, CustomerService customerService, WorkOrderService workOrderService)
{
    var summary = await analytics.GetSummaryAsync(await customerService.ListCustomersAsync(), await workOrderService.ListWorkOrdersAsync());
    return Results.Ok(summary);
}

async Task<IResult> GetAnalyticsByStatusAsync(AnalyticsService analytics, WorkOrderService service)
{
    var orders = await service.ListWorkOrdersAsync();
    var byStatus = analytics.GetByStatus(orders);
    return Results.Ok(byStatus);
}

async Task<IResult> GetCustomerAnalyticsAsync(int id, AnalyticsService analytics, CustomerService customerService, WorkOrderService workOrderService)
{
    var customer = await customerService.GetCustomerAsync(id);
    if (customer == null) return Results.NotFound();
    
    var orders = await workOrderService.GetWorkOrdersForCustomerAsync(id);
    return Results.Ok(new { customer = customer.ToResponse(), work_order_count = orders.Count, total_hours = orders.Count * 2 });
}

async Task<IResult> HealthCheckAsync()
{
    await Task.Delay(10);
    return Results.Ok(new { status = \"ServiceHub API v1.0 - Complete\", timestamp = DateTime.UtcNow });
}

async Task<IResult> InfoAsync()
{
    return Results.Ok(new 
    { 
        name = \"ServiceHub API\",
        version = \"1.0.0\",
        description = \"Complete REST API for service business management\",
        endpoints = new 
        {
            customers = \"GET /customers, POST /customers\",
            workorders = \"GET /workorders, POST /workorders, PUT /workorders/{id}/status\",
            analytics = \"GET /analytics/summary, GET /analytics/by-status\",
            health = \"GET /health\"
        }
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
    Task<List<WorkOrder>> GetByCustomerAsync(int customerId);
    Task<List<WorkOrder>> GetByStatusAsync(string status);
    Task<List<WorkOrder>> SearchAsync(string term);
    Task UpdateAsync(WorkOrder order);
}

// ========== REPOSITORIES ==========
class CustomerRepository : ICustomerRepository
{
    private List<Customer> customers = new();
    private int nextId = 1;
    
    public async Task AddCustomerAsync(Customer customer)
    {
        await Task.Delay(5);
        customer.Id = nextId++;
        customers.Add(customer);
    }
    
    public async Task<Customer?> GetCustomerAsync(int id)
    {
        await Task.Delay(5);
        return customers.FirstOrDefault(c => c.Id == id);
    }
    
    public async Task<List<Customer>> GetAllAsync()
    {
        await Task.Delay(5);
        return customers;
    }
}

class WorkOrderRepository : IWorkOrderRepository
{
    private List<WorkOrder> orders = new();
    private int nextId = 1;
    
    public async Task AddWorkOrderAsync(WorkOrder order)
    {
        await Task.Delay(5);
        order.Id = nextId++;
        orders.Add(order);
    }
    
    public async Task<WorkOrder?> GetWorkOrderAsync(int id)
    {
        await Task.Delay(5);
        return orders.FirstOrDefault(o => o.Id == id);
    }
    
    public async Task<List<WorkOrder>> GetAllAsync()
    {
        await Task.Delay(5);
        return orders;
    }
    
    public async Task<List<WorkOrder>> GetByCustomerAsync(int customerId)
    {
        await Task.Delay(5);
        return orders.Where(o => o.CustomerId == customerId).ToList();
    }
    
    public async Task<List<WorkOrder>> GetByStatusAsync(string status)
    {
        await Task.Delay(5);
        return orders.Where(o => o.Status == status).ToList();
    }
    
    public async Task<List<WorkOrder>> SearchAsync(string term)
    {
        await Task.Delay(5);
        return orders.Where(o => o.Description.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
    }
    
    public async Task UpdateAsync(WorkOrder order)
    {
        await Task.Delay(5);
        var existing = orders.FirstOrDefault(o => o.Id == order.Id);
        if (existing != null)
        {
            existing.Status = order.Status;
        }
    }
}

// ========== SERVICES ==========
class CustomerService
{
    private ICustomerRepository repository;
    public CustomerService(ICustomerRepository repo) => repository = repo;
    
    public async Task CreateCustomerAsync(Customer customer) => await repository.AddCustomerAsync(customer);
    public async Task<Customer?> GetCustomerAsync(int id) => await repository.GetCustomerAsync(id);
    public async Task<List<Customer>> ListCustomersAsync() => await repository.GetAllAsync();
}

class WorkOrderService
{
    private IWorkOrderRepository repository;
    public WorkOrderService(IWorkOrderRepository repo) => repository = repo;
    
    public async Task CreateWorkOrderAsync(WorkOrder order) => await repository.AddWorkOrderAsync(order);
    public async Task<WorkOrder?> GetWorkOrderAsync(int id) => await repository.GetWorkOrderAsync(id);
    public async Task<List<WorkOrder>> ListWorkOrdersAsync() => await repository.GetAllAsync();
    public async Task<List<WorkOrder>> GetWorkOrdersForCustomerAsync(int customerId) => await repository.GetByCustomerAsync(customerId);
    public async Task<List<WorkOrder>> GetWorkOrdersByStatusAsync(string status) => await repository.GetByStatusAsync(status);
    public async Task<List<WorkOrder>> SearchWorkOrdersAsync(string term) => await repository.SearchAsync(term);
    public async Task UpdateWorkOrderAsync(WorkOrder order) => await repository.UpdateAsync(order);
}

class AnalyticsService
{
    public async Task<object> GetSummaryAsync(List<Customer> customers, List<WorkOrder> orders)
    {
        await Task.Delay(10);
        return new
        {
            total_customers = customers.Count,
            total_workorders = orders.Count,
            scheduled = orders.Count(o => o.Status == \"Scheduled\"),
            in_progress = orders.Count(o => o.Status == \"InProgress\"),
            completed = orders.Count(o => o.Status == \"Completed\"),
            completion_rate = orders.Count > 0 ? Math.Round((double)orders.Count(o => o.Status == \"Completed\") / orders.Count * 100, 2) : 0
        };
    }
    
    public object GetByStatus(List<WorkOrder> orders)
    {
        return orders.GroupBy(o => o.Status)
            .Select(g => new { status = g.Key, count = g.Count() })
            .ToDictionary(x => x.status, x => x.count);
    }
}

// ========== DOMAIN MODELS ==========
class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = \"\";
    public string Email { get; set; } = \"\";
}

class WorkOrder
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string Description { get; set; } = \"\";
    public string Status { get; set; } = \"Scheduled\";
}

// ========== DTOs ==========
record CreateCustomerRequest(string Name, string Email);
record CustomerResponse(int Id, string Name, string Email);
record CreateWorkOrderRequest(int CustomerId, string Description, string Status);
record WorkOrderResponse(int Id, int CustomerId, string Description, string Status);
record UpdateWorkOrderStatusRequest(string Status);

// ========== MAPPERS ==========
static class Mappers
{
    public static Customer ToCustomer(this CreateCustomerRequest request) => new() { Name = request.Name, Email = request.Email };
    public static CustomerResponse ToResponse(this Customer customer) => new(customer.Id, customer.Name, customer.Email);
    public static WorkOrder ToWorkOrder(this CreateWorkOrderRequest request) => new() { CustomerId = request.CustomerId, Description = request.Description, Status = request.Status };
    public static WorkOrderResponse ToResponse(this WorkOrder order) => new(order.Id, order.CustomerId, order.Description, order.Status);
}
