using Microsoft.Extensions.Options;
using ServiceHub.Day16.Services;
using ServiceHub.Day16.Repositories;
using ServiceHub.Day16.Models;
using ServiceHub.Day16.Options;

// Day 16 — Options Pattern & Configuration
// Building on Day 15: Configuration management and Options pattern
// Demonstrates: appsettings.json, IOptions<T>, environment-specific settings, configuration hierarchies

var builder = WebApplication.CreateBuilder(args);

// ========== CONFIGURATION & OPTIONS ==========

// Configure options from appsettings.json
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings"));

// ========== DI SETUP (from Day 15) ==========

builder.Services.AddTransient<IValidationService, ValidationService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddSingleton<IMemoryCache>(new SimpleMemoryCache());
builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<WorkOrderService>();
builder.Services.AddScoped<OrderService>();

// Email service: Uses IOptions<EmailSettings>
if (builder.Environment.IsProduction())
    builder.Services.AddScoped<IEmailService, SmtpEmailService>();
else
    builder.Services.AddScoped<IEmailService, ConsoleEmailService>();

builder.Services.AddScoped<IRepositoryFactory>(sp =>
    new RepositoryFactory(
        sp.GetRequiredService<ICustomerRepository>(),
        sp.GetRequiredService<IWorkOrderRepository>()
    )
);

builder.Services.AddEndpointsApiExplorer();
// Swashbuckle removed to prefer built-in Scaler UI in .NET 10. If advanced OpenAPI features are required add Swashbuckle explicitly.

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ========== ENDPOINTS ==========

app.MapGet("/health", () => Results.Ok(new { status = "ok", version = "1.0" }))
    .WithName("Health")
    .WithOpenApi();

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
    var errors = validator.ValidateCustomer(request);
    if (errors.Any())
        return Results.BadRequest(new { errors });

    var customer = await service.CreateAsync(request.Name, request.Email);
    return Results.Created($"/customers/{customer.Id}", customer);
})
.WithName("CreateCustomer")
.WithOpenApi();

app.MapGet("/workorders", async (WorkOrderService service) =>
{
    var orders = await service.GetAllAsync();
    return Results.Ok(orders);
})
.WithName("GetWorkOrders")
.WithOpenApi();

app.MapPost("/workorders", async (CreateWorkOrderRequest request, OrderService orderService, IValidationService validator) =>
{
    var errors = validator.ValidateWorkOrder(request);
    if (errors.Any())
        return Results.BadRequest(new { errors });

    var order = await orderService.CreateOrderAsync(request.CustomerId, request.Description);
    return Results.Created($"/workorders/{order.Id}", order);
})
.WithName("CreateWorkOrder")
.WithOpenApi();

// Configuration info endpoint
app.MapGet("/config-info", (IOptions<EmailSettings> emailOptions, IOptions<AppSettings> appOptions) =>
{
    return Results.Ok(new
    {
        environment = app.Environment.EnvironmentName,
        appVersion = appOptions.Value.ApiVersion,
        emailProvider = emailOptions.Value.Provider,
        configurationLoaded = true
    });
})
.WithName("ConfigInfo")
.WithOpenApi();

app.Run();

// ========== MODELS ==========

namespace ServiceHub.Day16.Models
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

// ========== OPTIONS CLASSES ==========

namespace ServiceHub.Day16.Options
{
    public class EmailSettings
    {
        public string Provider { get; set; } = "Console";
        public string SmtpServer { get; set; } = "smtp.gmail.com";
        public int SmtpPort { get; set; } = 587;
        public string ApiKey { get; set; } = "";
    }

    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = "";
        public string Provider { get; set; } = "InMemory";
    }

    public class AppSettings
    {
        public string ApiVersion { get; set; } = "1.0.0";
        public string AppName { get; set; } = "ServiceHub";
        public string Environment { get; set; } = "Development";
    }
}

// ========== REPOSITORIES & INTERFACES ==========

namespace ServiceHub.Day16.Repositories
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

namespace ServiceHub.Day16.Services
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
        public string GetSetting(string key) => key switch
        {
            "demo" => "This is configured from appsettings.json",
            _ => "Unknown setting"
        };
    }

    public class ConsoleEmailService : IEmailService
    {
        private readonly IOptions<EmailSettings> _emailOptions;

        public ConsoleEmailService(IOptions<EmailSettings> emailOptions)
        {
            _emailOptions = emailOptions;
        }

        public Task SendAsync(string to, string subject, string message)
        {
            var settings = _emailOptions.Value;
            Console.WriteLine($"📧 [{settings.Provider}] Email to {to}: {subject}");
            return Task.CompletedTask;
        }
    }

    public class SmtpEmailService : IEmailService
    {
        private readonly IOptions<EmailSettings> _emailOptions;

        public SmtpEmailService(IOptions<EmailSettings> emailOptions)
        {
            _emailOptions = emailOptions;
        }

        public Task SendAsync(string to, string subject, string message)
        {
            var settings = _emailOptions.Value;
            Console.WriteLine($"📧 [SMTP {settings.SmtpServer}:{settings.SmtpPort}] Email to {to}: {subject}");
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
            var customer = await _customers.GetAsync(customerId);
            if (customer == null)
                throw new InvalidOperationException($"Customer {customerId} not found");

            var order = new WorkOrder { CustomerId = customerId, Description = description };
            await _orders.AddAsync(order);

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
