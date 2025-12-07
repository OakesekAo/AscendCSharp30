using System;
using System.Collections.Generic;
using System.Linq;

var builder = WebApplicationBuilder.CreateBuilder(args);

// ========== DEPENDENCY INJECTION ==========
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<WorkOrderService>();

var app = builder.Build();

app.UseHttpsRedirection();

// ========== CUSTOMER ENDPOINTS (WITH DTOs) ==========
app.MapGet("/customers", (CustomerService service) =>
{
    var customers = service.ListCustomers();
    var responses = customers.Select(c => c.ToResponse()).ToList();
    return Results.Ok(responses);
})
.WithName("GetCustomers")
.WithOpenApi();

app.MapGet("/customers/{id}", (int id, CustomerService service) =>
{
    var customer = service.GetCustomer(id);
    return customer != null 
        ? Results.Ok(customer.ToResponse()) 
        : Results.NotFound();
})
.WithName("GetCustomerById")
.WithOpenApi();

app.MapPost("/customers", (CreateCustomerRequest request, CustomerService service) =>
{
    var customer = request.ToCustomer();
    service.CreateCustomer(customer.Id, customer.Name, customer.Email);
    return Results.Created($"/customers/{customer.Id}", customer.ToResponse());
})
.WithName("CreateCustomer")
.WithOpenApi();

// ========== WORK ORDER ENDPOINTS (WITH DTOs) ==========
app.MapGet("/workorders", (WorkOrderService service) =>
{
    var orders = service.ListWorkOrders();
    var responses = orders.Select(o => o.ToResponse()).ToList();
    return Results.Ok(responses);
})
.WithName("GetWorkOrders")
.WithOpenApi();

app.MapGet("/workorders/{id}", (int id, WorkOrderService service) =>
{
    var order = service.GetWorkOrder(id);
    return order != null 
        ? Results.Ok(order.ToResponse()) 
        : Results.NotFound();
})
.WithName("GetWorkOrderById")
.WithOpenApi();

app.MapPost("/workorders", (CreateWorkOrderRequest request, WorkOrderService service) =>
{
    var order = request.ToWorkOrder();
    service.CreateWorkOrder(order.Id, order.CustomerId, order.Description, order.Status);
    return Results.Created($"/workorders/{order.Id}", order.ToResponse());
})
.WithName("CreateWorkOrder")
.WithOpenApi();

app.MapGet("/health", () => Results.Ok(new { status = "ServiceHub API v0.2", timestamp = DateTime.UtcNow }))
.WithName("Health")
.WithOpenApi();

app.Run();

// ========== INTERFACES ==========
interface ICustomerRepository
{
    void AddCustomer(Customer customer);
    Customer? GetCustomer(int id);
    List<Customer> GetAll();
}

interface IWorkOrderRepository
{
    void AddWorkOrder(WorkOrder order);
    WorkOrder? GetWorkOrder(int id);
    List<WorkOrder> GetAll();
}

// ========== REPOSITORIES ==========
class CustomerRepository : ICustomerRepository
{
    private List<Customer> customers = new();
    private int nextId = 1;
    
    public void AddCustomer(Customer customer)
    {
        customer.Id = nextId++;
        customers.Add(customer);
    }
    
    public Customer? GetCustomer(int id) => customers.FirstOrDefault(c => c.Id == id);
    public List<Customer> GetAll() => customers;
}

class WorkOrderRepository : IWorkOrderRepository
{
    private List<WorkOrder> orders = new();
    private int nextId = 1;
    
    public void AddWorkOrder(WorkOrder order)
    {
        order.Id = nextId++;
        orders.Add(order);
    }
    
    public WorkOrder? GetWorkOrder(int id) => orders.FirstOrDefault(o => o.Id == id);
    public List<WorkOrder> GetAll() => orders;
}

// ========== SERVICES ==========
class CustomerService
{
    private ICustomerRepository repository;
    public CustomerService(ICustomerRepository repo) => repository = repo;
    
    public void CreateCustomer(int id, string name, string email)
    {
        var customer = new Customer { Id = id, Name = name, Email = email };
        repository.AddCustomer(customer);
    }
    
    public Customer? GetCustomer(int id) => repository.GetCustomer(id);
    public List<Customer> ListCustomers() => repository.GetAll();
}

class WorkOrderService
{
    private IWorkOrderRepository repository;
    public WorkOrderService(IWorkOrderRepository repo) => repository = repo;
    
    public void CreateWorkOrder(int id, int customerId, string description, string status)
    {
        var order = new WorkOrder { Id = id, CustomerId = customerId, Description = description, Status = status };
        repository.AddWorkOrder(order);
    }
    
    public WorkOrder? GetWorkOrder(int id) => repository.GetWorkOrder(id);
    public List<WorkOrder> ListWorkOrders() => repository.GetAll();
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
