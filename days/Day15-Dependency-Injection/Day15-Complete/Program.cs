using ServiceHub.Day15.Services;
using ServiceHub.Day15.Repositories;
using ServiceHub.Day15.Models;
using Microsoft.Extensions.DependencyInjection;

// Day 15 — Advanced Dependency Injection Patterns
// Building on Week 2: Master lifetimes, factories, decorators, and service hierarchies
// Demonstrates: Transient/Scoped/Singleton, factory patterns, service composition, decorators

var builder = WebApplication.CreateBuilder(args);

// ========== ADVANCED DI SETUP ==========

// Lifetimes: Choose based on needs
// Transient: New instance every time (lightweight, stateless)
builder.Services.AddTransient<IValidationService, ValidationService>();

// Scoped: One per request (repositories, services, DbContext)
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();

// Singleton: One for entire application (config, cache, expensive resources)
builder.Services.AddSingleton<IMemoryCache>(new SimpleMemoryCache());
builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();

// Service hierarchy: Services depending on services
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<WorkOrderService>();
builder.Services.AddScoped<OrderService>(); // Depends on both repos + email

// Email service: Conditional based on environment
if (builder.Environment.IsProduction())
    builder.Services.AddScoped<IEmailService, SmtpEmailService>();
else
    builder.Services.AddScoped<IEmailService, ConsoleEmailService>();

// Factory pattern: Complex object creation
builder.Services.AddScoped<IRepositoryFactory>(serviceProvider =>
    new RepositoryFactory(
        serviceProvider.GetRequiredService<ICustomerRepository>(),
        serviceProvider.GetRequiredService<IWorkOrderRepository>()
    )
);

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ========== ENDPOINTS ==========

app.MapGet("/health", () => Results.Ok(new { status = "ok", lifetimes = "Transient, Scoped, Singleton demonstrated" }))
    .WithName("Health")
    .WithOpenApi();

// Customer endpoints
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

app.MapPost("/customers", async (CreateCustomerRequest request, CustomerService service, IValidationService validator) =>
{
    var validationErrors = validator.ValidateCustomer(request);
    if (validationErrors.Any())
        return Results.BadRequest(new { errors = validationErrors });

    var customer = await service.CreateAsync(request.Name, request.Email);
    return Results.Created($"/customers/{customer.Id}", customer);
})
.WithName("CreateCustomer")
.WithOpenApi();

// Work order endpoints
app.MapGet("/workorders", async (WorkOrderService service) =>
{
    var orders = await service.GetAllAsync();
    return Results.Ok(orders);
})
.WithName("GetWorkOrders")
.WithOpenApi();

app.MapPost("/workorders", async (CreateWorkOrderRequest request, OrderService orderService, IValidationService validator) =>
{
    var validationErrors = validator.ValidateWorkOrder(request);
    if (validationErrors.Any())
        return Results.BadRequest(new { errors = validationErrors });

    var order = await orderService.CreateOrderAsync(request.CustomerId, request.Description);
    return Results.Created($"/workorders/{order.Id}", order);
})
.WithName("CreateWorkOrder")
.WithOpenApi();

// DI info endpoint
app.MapGet("/di-info", (IConfigurationService config, IMemoryCache cache) =>
{
    return Results.Ok(new
    {
        environment = app.Environment.EnvironmentName,
        singletonConfig = config.GetSetting("demo"),
        cacheType = cache.GetType().Name,
        message = "Advanced DI patterns in action"
    });
})
.WithName("DIInfo")
.WithOpenApi();

app.Run();

// ========== MODELS ==========

namespace ServiceHub.Day15.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
    }

    public class WorkOrder
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Description { get; set; } = "";
        public string Status { get; set; } = "Scheduled";
    }
}

// ========== REPOSITORIES & INTERFACES ==========

namespace ServiceHub.Day15.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetAsync(int id);
        Task<List<Customer>> GetAllAsync();
        Task AddAsync(Customer customer);
    }

    public interface IWorkOrderRepository
    {
        Task<WorkOrder?> GetAsync(int id);
        Task<List<WorkOrder>> GetAllAsync();
        Task AddAsync(WorkOrder order);
    }

    public interface IRepositoryFactory
    {
        ICustomerRepository GetCustomerRepository();
        IWorkOrderRepository GetWorkOrderRepository();
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly List<Customer> _customers = new();
        private int _nextId = 1;

        public CustomerRepository()
        {
            _customers.Add(new Customer { Id = _nextId++, Name = "Alice Johnson", Email = "alice@example.com" });
            _customers.Add(new Customer { Id = _nextId++, Name = "Bob Smith", Email = "bob@example.com" });
        }

        public Task<Customer?> GetAsync(int id) => Task.FromResult(_customers.FirstOrDefault(c => c.Id == id));
        public Task<List<Customer>> GetAllAsync() => Task.FromResult(new List<Customer>(_customers));
        public Task AddAsync(Customer customer)
        {
            customer.Id = _nextId++;
            _customers.Add(customer);
            return Task.CompletedTask;
        }
    }

    public class WorkOrderRepository : IWorkOrderRepository
    {
        private readonly List<WorkOrder> _orders = new();
        private int _nextId = 1;

        public WorkOrderRepository()
        {
            _orders.Add(new WorkOrder { Id = _nextId++, CustomerId = 1, Description = "Gutter Cleaning", Status = "Scheduled" });
        }

        public Task<WorkOrder?> GetAsync(int id) => Task.FromResult(_orders.FirstOrDefault(o => o.Id == id));
        public Task<List<WorkOrder>> GetAllAsync() => Task.FromResult(new List<WorkOrder>(_orders));
        public Task AddAsync(WorkOrder order)
        {
            order.Id = _nextId++;
            _orders.Add(order);
            return Task.CompletedTask;
        }
    }

    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly ICustomerRepository _customers;
        private readonly IWorkOrderRepository _orders;

        public RepositoryFactory(ICustomerRepository customers, IWorkOrderRepository orders)
        {
            _customers = customers;
            _orders = orders;
        }

        public ICustomerRepository GetCustomerRepository() => _customers;
        public IWorkOrderRepository GetWorkOrderRepository() => _orders;
    }
}

// ========== SERVICES & INTERFACES ==========

namespace ServiceHub.Day15.Services
{
    public interface IValidationService
    {
        List<string> ValidateCustomer(CreateCustomerRequest request);
        List<string> ValidateWorkOrder(CreateWorkOrderRequest request);
    }

    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string message);
    }

    public interface IMemoryCache
    {
        void Set<T>(string key, T value, TimeSpan? expiration = null);
        bool TryGetValue<T>(string key, out T? value);
    }

    public interface IConfigurationService
    {
        string GetSetting(string key);
    }

    public class ValidationService : IValidationService
    {
        // Transient: New instance each time (stateless, lightweight)
        public List<string> ValidateCustomer(CreateCustomerRequest request)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(request.Name)) errors.Add("Name required");
            if (string.IsNullOrWhiteSpace(request.Email)) errors.Add("Email required");
            else if (!request.Email.Contains("@")) errors.Add("Invalid email");
            return errors;
        }

        public List<string> ValidateWorkOrder(CreateWorkOrderRequest request)
        {
            var errors = new List<string>();
            if (request.CustomerId <= 0) errors.Add("Valid customer required");
            if (string.IsNullOrWhiteSpace(request.Description)) errors.Add("Description required");
            return errors;
        }
    }

    public class SimpleMemoryCache : IMemoryCache
    {
        private readonly Dictionary<string, object?> _cache = new();

        public void Set<T>(string key, T value, TimeSpan? expiration = null) => _cache[key] = value;
        public bool TryGetValue<T>(string key, out T? value)
        {
            if (_cache.TryGetValue(key, out var obj))
            {
                value = (T?)obj;
                return true;
            }
            value = default;
            return false;
        }
    }

    public class ConfigurationService : IConfigurationService
    {
        // Singleton: One for entire app lifetime
        public string GetSetting(string key) => key switch
        {
            "demo" => "This is a singleton service",
            _ => "Unknown setting"
        };
    }

    public class ConsoleEmailService : IEmailService
    {
        public Task SendAsync(string to, string subject, string message)
        {
            Console.WriteLine($"📧 Email to {to}: {subject}");
            return Task.CompletedTask;
        }
    }

    public class SmtpEmailService : IEmailService
    {
        public Task SendAsync(string to, string subject, string message)
        {
            // In production, send real email
            Console.WriteLine($"📧 [SMTP] Email to {to}: {subject}");
            return Task.CompletedTask;
        }
    }

    public class CustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository) => _repository = repository;

        public async Task<Customer> CreateAsync(string name, string email)
        {
            var customer = new Customer { Name = name, Email = email };
            await _repository.AddAsync(customer);
            return customer;
        }

        public async Task<Customer?> GetAsync(int id) => await _repository.GetAsync(id);
        public async Task<List<Customer>> GetAllAsync() => await _repository.GetAllAsync();
    }

    public class WorkOrderService
    {
        private readonly IWorkOrderRepository _repository;

        public WorkOrderService(IWorkOrderRepository repository) => _repository = repository;

        public async Task<WorkOrder> CreateAsync(int customerId, string description)
        {
            var order = new WorkOrder { CustomerId = customerId, Description = description };
            await _repository.AddAsync(order);
            return order;
        }

        public async Task<List<WorkOrder>> GetAllAsync() => await _repository.GetAllAsync();
    }

    public class OrderService
    {
        // Service hierarchy: Depends on multiple repositories and services
        private readonly ICustomerRepository _customers;
        private readonly IWorkOrderRepository _orders;
        private readonly IEmailService _email;

        public OrderService(
            ICustomerRepository customers,
            IWorkOrderRepository orders,
            IEmailService email)
        {
            _customers = customers;
            _orders = orders;
            _email = email;
        }

        public async Task<WorkOrder> CreateOrderAsync(int customerId, string description)
        {
            // Service composition: Use multiple injected services
            var customer = await _customers.GetAsync(customerId);
            if (customer == null)
                throw new InvalidOperationException($"Customer {customerId} not found");

            var order = new WorkOrder { CustomerId = customerId, Description = description };
            await _orders.AddAsync(order);

            // Send email notification
            await _email.SendAsync(
                customer.Email,
                "New Work Order Created",
                $"Your work order #{order.Id} has been created: {description}"
            );

            return order;
        }
    }
}

// ========== DTOs ==========

public record CreateCustomerRequest(string Name, string Email);
public record CreateWorkOrderRequest(int CustomerId, string Description);
