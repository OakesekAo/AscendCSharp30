using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

// Day 08 — Dependency Injection: Complete Example
// ServiceHub with DI container, repositories, and services
// Demonstrates: Interfaces, constructor injection, DI registration

Console.WriteLine("╔════════════════════════════════════════╗");
Console.WriteLine("║  ServiceHub - Dependency Injection v1  ║");
Console.WriteLine("╚════════════════════════════════════════╝\n");

// ========== SETUP: DI CONTAINER ==========
var services = new ServiceCollection();

// Register repositories
services.AddScoped<ICustomerRepository, CustomerRepository>();
services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();

// Register services
services.AddScoped<CustomerService>();
services.AddScoped<WorkOrderService>();

var provider = services.BuildServiceProvider();

// ========== GET SERVICES FROM DI CONTAINER ==========
var customerService = provider.GetRequiredService<CustomerService>();
var workOrderService = provider.GetRequiredService<WorkOrderService>();

// ========== USE SERVICES ==========
Console.WriteLine("--- Adding Customers ---");
customerService.CreateCustomer(1, "Alice Johnson", "alice@example.com");
customerService.CreateCustomer(2, "Bob Smith", "bob@example.com");
customerService.CreateCustomer(3, "Charlie Brown", "charlie@example.com");
Console.WriteLine();

Console.WriteLine("--- All Customers ---");
foreach (var customer in customerService.ListCustomers())
{
    Console.WriteLine($"• ID {customer.Id}: {customer.Name} ({customer.Email})");
}
Console.WriteLine();

Console.WriteLine("--- Adding Work Orders ---");
workOrderService.CreateWorkOrder(1, 1, "Gutter Cleaning", "Scheduled");
workOrderService.CreateWorkOrder(2, 2, "Lawn Mowing", "Scheduled");
workOrderService.CreateWorkOrder(3, 1, "Window Washing", "Scheduled");
Console.WriteLine();

Console.WriteLine("--- All Work Orders ---");
foreach (var order in workOrderService.ListWorkOrders())
{
    Console.WriteLine($"• ID {order.Id}: {order.Description} for Customer {order.CustomerId} ({order.Status})");
}
Console.WriteLine();

Console.WriteLine("--- Find Customer ---");
var found = customerService.GetCustomer(1);
if (found != null)
{
    Console.WriteLine($"Found: {found.Name}");
}
Console.WriteLine();

Console.WriteLine("✅ Day 08 Complete!");

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

// ========== IMPLEMENTATIONS ==========
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

// ========== SERVICES (WITH INJECTED DEPENDENCIES) ==========
class CustomerService
{
    private ICustomerRepository repository;
    
    // Constructor injection
    public CustomerService(ICustomerRepository repo)
    {
        repository = repo;
    }
    
    public void CreateCustomer(int id, string name, string email)
    {
        var customer = new Customer { Id = id, Name = name, Email = email };
        repository.AddCustomer(customer);
        Console.WriteLine($"  ✓ Created customer: {name}");
    }
    
    public Customer? GetCustomer(int id) => repository.GetCustomer(id);
    public List<Customer> ListCustomers() => repository.GetAll();
}

class WorkOrderService
{
    private IWorkOrderRepository repository;
    
    // Constructor injection
    public WorkOrderService(IWorkOrderRepository repo)
    {
        repository = repo;
    }
    
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
        Console.WriteLine($"  ✓ Created work order: {description}");
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
