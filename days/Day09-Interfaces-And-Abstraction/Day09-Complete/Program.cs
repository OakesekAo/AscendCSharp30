using System;
using System.Collections.Generic;
using System.Linq;

var builder = WebApplicationBuilder.CreateBuilder(args);

// ========== DEPENDENCY INJECTION SETUP ==========
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<WorkOrderService>();

var app = builder.Build();

// ========== MIDDLEWARE ==========
app.UseHttpsRedirection();

// ========== CUSTOMER ENDPOINTS ==========
app.MapGet("/customers", (CustomerService service) =>
{
    return Results.Ok(service.ListCustomers());
})
.WithName("GetCustomers")
.WithOpenApi()
.Produces<List<Customer>>(StatusCodes.Status200OK);

app.MapGet("/customers/{id}", (int id, CustomerService service) =>
{
    var customer = service.GetCustomer(id);
    return customer != null 
        ? Results.Ok(customer) 
        : Results.NotFound($"Customer {id} not found");
})
.WithName("GetCustomerById")
.WithOpenApi();

app.MapPost("/customers", (Customer customer, CustomerService service) =>
{
    service.CreateCustomer(customer.Id, customer.Name, customer.Email);
    return Results.Created($"/customers/{customer.Id}", customer);
})
.WithName("CreateCustomer")
.WithOpenApi()
.Produces<Customer>(StatusCodes.Status201Created);

// ========== WORK ORDER ENDPOINTS ==========
app.MapGet("/workorders", (WorkOrderService service) =>
{
    return Results.Ok(service.ListWorkOrders());
})
.WithName("GetWorkOrders")
.WithOpenApi()
.Produces<List<WorkOrder>>(StatusCodes.Status200OK);

app.MapGet("/workorders/{id}", (int id, WorkOrderService service) =>
{
    var order = service.GetWorkOrder(id);
    return order != null 
        ? Results.Ok(order) 
        : Results.NotFound($"Work order {id} not found");
})
.WithName("GetWorkOrderById")
.WithOpenApi();

app.MapPost("/workorders", (WorkOrder order, WorkOrderService service) =>
{
    service.CreateWorkOrder(order.Id, order.CustomerId, order.Description, order.Status);
    return Results.Created($"/workorders/{order.Id}", order);
})
.WithName("CreateWorkOrder")
.WithOpenApi()
.Produces<WorkOrder>(StatusCodes.Status201Created);

// ========== HEALTH CHECK ==========
app.MapGet("/health", () => Results.Ok(new { status = "ServiceHub API running", timestamp = DateTime.UtcNow }))
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
        var order = new WorkOrder 
        { 
            Id = id, 
            CustomerId = customerId, 
            Description = description, 
            Status = status 
        };
        repository.AddWorkOrder(order);
    }
    
    public WorkOrder? GetWorkOrder(int id) => repository.GetWorkOrder(id);
    public List<WorkOrder> ListWorkOrders() => repository.GetAll();
}

// ========== DATA MODELS ==========
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
