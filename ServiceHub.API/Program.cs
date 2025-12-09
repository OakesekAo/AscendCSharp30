using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplicationBuilder.CreateBuilder(args);

// ========== DEPENDENCY INJECTION SETUP ==========
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<WorkOrderService>();
builder.Services.AddScoped<AnalyticsService>();

// Swagger/OpenAPI
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

// ========== HEALTH CHECK ==========
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

// ========== CUSTOMER ENDPOINTS ==========
app.MapGet("/customers", (CustomerService service) =>
{
    return Results.Ok(service.ListCustomers());
})
.WithName("GetCustomers")
.WithOpenApi()
.Produces<List<CustomerResponse>>(StatusCodes.Status200OK);

app.MapGet("/customers/{id}", (int id, CustomerService service) =>
{
    var customer = service.GetCustomer(id);
    return customer != null 
        ? Results.Ok(customer.ToResponse()) 
        : Results.NotFound(new { error = $"Customer {id} not found" });
})
.WithName("GetCustomerById")
.WithOpenApi();

app.MapPost("/customers", (CreateCustomerRequest request, CustomerService service) =>
{
    if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email))
        return Results.BadRequest(new { error = "Name and Email are required" });
    
    var customer = new Customer { Name = request.Name, Email = request.Email };
    service.CreateCustomer(customer);
    return Results.Created($"/customers/{customer.Id}", customer.ToResponse());
})
.WithName("CreateCustomer")
.WithOpenApi();

// ========== WORK ORDER ENDPOINTS ==========
app.MapGet("/workorders", (WorkOrderService service) =>
{
    return Results.Ok(service.ListWorkOrders());
})
.WithName("GetWorkOrders")
.WithOpenApi();

app.MapGet("/workorders/{id}", (int id, WorkOrderService service) =>
{
    var order = service.GetWorkOrder(id);
    return order != null 
        ? Results.Ok(order.ToResponse()) 
        : Results.NotFound(new { error = $"Work order {id} not found" });
})
.WithName("GetWorkOrderById")
.WithOpenApi();

app.MapPost("/workorders", (CreateWorkOrderRequest request, WorkOrderService service) =>
{
    if (request.CustomerId <= 0 || string.IsNullOrWhiteSpace(request.Description))
        return Results.BadRequest(new { error = "CustomerId and Description are required" });
    
    var order = new WorkOrder 
    { 
        CustomerId = request.CustomerId, 
        Description = request.Description, 
        Status = "Scheduled" 
    };
    service.CreateWorkOrder(order);
    return Results.Created($"/workorders/{order.Id}", order.ToResponse());
})
.WithName("CreateWorkOrder")
.WithOpenApi();

app.MapGet("/workorders/by-customer/{customerId}", (int customerId, WorkOrderService service) =>
{
    var orders = service.GetWorkOrdersByCustomer(customerId);
    return Results.Ok(orders.Select(o => o.ToResponse()).ToList());
})
.WithName("GetWorkOrdersByCustomer")
.WithOpenApi();

app.MapPut("/workorders/{id}/status", (int id, UpdateWorkOrderStatusRequest request, WorkOrderService service) =>
{
    var order = service.GetWorkOrder(id);
    if (order == null)
        return Results.NotFound(new { error = $"Work order {id} not found" });
    
    var validStatuses = new[] { "Scheduled", "InProgress", "Completed", "Canceled" };
    if (!validStatuses.Contains(request.Status))
        return Results.BadRequest(new { error = "Invalid status" });
    
    order.Status = request.Status;
    service.UpdateWorkOrder(order);
    return Results.Ok(order.ToResponse());
})
.WithName("UpdateWorkOrderStatus")
.WithOpenApi();

// ========== ANALYTICS ENDPOINTS ==========
app.MapGet("/analytics/summary", async (AnalyticsService analytics, CustomerService customerService, WorkOrderService orderService) =>
{
    var summary = await analytics.GetSummaryAsync(customerService.ListCustomers(), orderService.ListWorkOrders());
    return Results.Ok(summary);
})
.WithName("GetAnalyticsSummary")
.WithOpenApi();

app.Run();

// ========== INTERFACES ==========
interface ICustomerRepository
{
    void Add(Customer customer);
    Customer? Get(int id);
    List<Customer> GetAll();
}

interface IWorkOrderRepository
{
    void Add(WorkOrder order);
    WorkOrder? Get(int id);
    List<WorkOrder> GetAll();
    List<WorkOrder> GetByCustomerId(int customerId);
    void Update(WorkOrder order);
}

// ========== REPOSITORIES ==========
class CustomerRepository : ICustomerRepository
{
    private List<Customer> customers = new();
    
    public CustomerRepository()
    {
        // Seed data
        Add(new Customer { Name = "Alice Johnson", Email = "alice@example.com" });
        Add(new Customer { Name = "Bob Smith", Email = "bob@example.com" });
        Add(new Customer { Name = "Charlie Brown", Email = "charlie@example.com" });
    }
    
    public void Add(Customer customer)
    {
        customer.Id = customers.Count > 0 ? customers.Max(c => c.Id) + 1 : 1;
        customers.Add(customer);
    }
    
    public Customer? Get(int id) => customers.FirstOrDefault(c => c.Id == id);
    public List<Customer> GetAll() => customers;
}

class WorkOrderRepository : IWorkOrderRepository
{
    private List<WorkOrder> orders = new();
    
    public WorkOrderRepository()
    {
        // Seed data
        Add(new WorkOrder { CustomerId = 1, Description = "Gutter Cleaning", Status = "Scheduled" });
        Add(new WorkOrder { CustomerId = 2, Description = "Lawn Mowing", Status = "Scheduled" });
        Add(new WorkOrder { CustomerId = 1, Description = "Window Washing", Status = "InProgress" });
        Add(new WorkOrder { CustomerId = 3, Description = "Pressure Washing", Status = "Scheduled" });
    }
    
    public void Add(WorkOrder order)
    {
        order.Id = orders.Count > 0 ? orders.Max(o => o.Id) + 1 : 1;
        orders.Add(order);
    }
    
    public WorkOrder? Get(int id) => orders.FirstOrDefault(o => o.Id == id);
    public List<WorkOrder> GetAll() => orders;
    public List<WorkOrder> GetByCustomerId(int customerId) => orders.Where(o => o.CustomerId == customerId).ToList();
    public void Update(WorkOrder order)
    {
        var existing = Get(order.Id);
        if (existing != null)
        {
            existing.Status = order.Status;
            existing.Description = order.Description;
        }
    }
}

// ========== SERVICES ==========
class CustomerService
{
    private readonly ICustomerRepository repository;
    
    public CustomerService(ICustomerRepository repo) => repository = repo;
    
    public void CreateCustomer(Customer customer) => repository.Add(customer);
    public Customer? GetCustomer(int id) => repository.Get(id);
    public List<Customer> ListCustomers() => repository.GetAll();
}

class WorkOrderService
{
    private readonly IWorkOrderRepository repository;
    
    public WorkOrderService(IWorkOrderRepository repo) => repository = repo;
    
    public void CreateWorkOrder(WorkOrder order) => repository.Add(order);
    public WorkOrder? GetWorkOrder(int id) => repository.Get(id);
    public List<WorkOrder> ListWorkOrders() => repository.GetAll();
    public List<WorkOrder> GetWorkOrdersByCustomer(int customerId) => repository.GetByCustomerId(customerId);
    public void UpdateWorkOrder(WorkOrder order) => repository.Update(order);
}

class AnalyticsService
{
    public async Task<object> GetSummaryAsync(List<Customer> customers, List<WorkOrder> orders)
    {
        await Task.Delay(10);
        
        var completed = orders.Count(o => o.Status == "Completed");
        var total = orders.Count;
        var completionRate = total > 0 ? Math.Round((double)completed / total * 100, 2) : 0;
        
        return new
        {
            timestamp = DateTime.UtcNow,
            total_customers = customers.Count,
            total_workorders = total,
            by_status = new
            {
                scheduled = orders.Count(o => o.Status == "Scheduled"),
                in_progress = orders.Count(o => o.Status == "InProgress"),
                completed = completed,
                canceled = orders.Count(o => o.Status == "Canceled")
            },
            completion_rate_percent = completionRate
        };
    }
}

// ========== DATA MODELS ==========
class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    
    public CustomerResponse ToResponse() => new(Id, Name, Email);
}

class WorkOrder
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string Description { get; set; } = "";
    public string Status { get; set; } = "Scheduled";
    
    public WorkOrderResponse ToResponse() => new(Id, CustomerId, Description, Status);
}

// ========== DTOs (API Contracts) ==========
record CustomerResponse(int Id, string Name, string Email);
record WorkOrderResponse(int Id, int CustomerId, string Description, string Status);

record CreateCustomerRequest(string Name, string Email);
record CreateWorkOrderRequest(int CustomerId, string Description);
record UpdateWorkOrderStatusRequest(string Status);
