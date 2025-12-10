using Microsoft.Extensions.Options;
using Serilog;
using ServiceHub.Day17.Services;
using ServiceHub.Day17.Repositories;
using ServiceHub.Day17.Models;
using ServiceHub.Day17.Options;

// Day 17 — Logging with Serilog
// Building on Day 16: Structured logging and production logging patterns
// Demonstrates: Serilog setup, structured logging, log levels, file output

var builder = WebApplication.CreateBuilder(args);

// ========== SERILOG CONFIGURATION ==========
builder.Host.UseSerilog((context, config) =>
    config
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithThreadId());

// ========== CONFIGURATION & OPTIONS ==========
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings"));

// ========== DI SETUP ==========
builder.Services.AddTransient<IValidationService, ValidationService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddSingleton<IMemoryCache>(new SimpleMemoryCache());
builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<WorkOrderService>();
builder.Services.AddScoped<OrderService>();

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
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Log startup info
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Starting ServiceHub API - Environment: {Environment}", app.Environment.EnvironmentName);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    logger.LogInformation("Swagger enabled at /swagger");
}

app.UseHttpsRedirection();

// ========== ENDPOINTS ==========

app.MapGet("/health", () =>
{
    logger.LogDebug("Health check requested");
    return Results.Ok(new { status = "ok", version = "1.0", timestamp = DateTime.UtcNow });
})
.WithName("Health")
.WithOpenApi();

app.MapGet("/customers", async (CustomerService service, ILogger<Program> svcLogger) =>
{
    svcLogger.LogInformation("Fetching all customers");
    var customers = await service.GetAllAsync();
    svcLogger.LogInformation("Retrieved {CustomerCount} customers", customers.Count);
    return Results.Ok(customers);
})
.WithName("GetCustomers")
.WithOpenApi();

app.MapGet("/customers/{id}", async (int id, CustomerService service, ILogger<Program> svcLogger) =>
{
    svcLogger.LogDebug("Fetching customer {CustomerId}", id);
    var customer = await service.GetAsync(id);
    if (customer != null)
        svcLogger.LogInformation("Customer {CustomerId} found: {CustomerName}", id, customer.Name);
    else
        svcLogger.LogWarning("Customer {CustomerId} not found", id);
    
    return customer != null ? Results.Ok(customer) : Results.NotFound();
})
.WithName("GetCustomerById")
.WithOpenApi();

app.MapPost("/customers", async (CreateCustomerRequest request, CustomerService service, IValidationService validator, ILogger<Program> svcLogger) =>
{
    svcLogger.LogInformation("Creating customer request: {Name}", request.Name);
    var errors = validator.ValidateCustomer(request);
    if (errors.Any())
    {
        svcLogger.LogWarning("Customer validation failed: {Errors}", string.Join(", ", errors));
        return Results.BadRequest(new { errors });
    }

    var customer = await service.CreateAsync(request.Name, request.Email);
    svcLogger.LogInformation("Customer created successfully: {CustomerId} - {Name}", customer.Id, customer.Name);
    return Results.Created($"/customers/{customer.Id}", customer);
})
.WithName("CreateCustomer")
.WithOpenApi();

app.MapGet("/workorders", async (WorkOrderService service, ILogger<Program> svcLogger) =>
{
    svcLogger.LogInformation("Fetching all work orders");
    var orders = await service.GetAllAsync();
    svcLogger.LogInformation("Retrieved {WorkOrderCount} work orders", orders.Count);
    return Results.Ok(orders);
})
.WithName("GetWorkOrders")
.WithOpenApi();

app.MapPost("/workorders", async (CreateWorkOrderRequest request, OrderService orderService, IValidationService validator, ILogger<Program> svcLogger) =>
{
    svcLogger.LogInformation("Creating work order for customer {CustomerId}: {Description}", request.CustomerId, request.Description);
    var errors = validator.ValidateWorkOrder(request);
    if (errors.Any())
    {
        svcLogger.LogWarning("Work order validation failed: {Errors}", string.Join(", ", errors));
        return Results.BadRequest(new { errors });
    }

    try
    {
        var order = await orderService.CreateOrderAsync(request.CustomerId, request.Description);
        svcLogger.LogInformation("Work order created: {WorkOrderId} for customer {CustomerId}", order.Id, request.CustomerId);
        return Results.Created($"/workorders/{order.Id}", order);
    }
    catch (Exception ex)
    {
        svcLogger.LogError(ex, "Failed to create work order for customer {CustomerId}", request.CustomerId);
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("CreateWorkOrder")
.WithOpenApi();

app.MapGet("/logs-info", (IOptions<AppSettings> appOptions) =>
{
    logger.LogDebug("Logs info endpoint called");
    return Results.Ok(new
    {
        loggingEnabled = true,
        framework = "Serilog",
        environment = app.Environment.EnvironmentName,
        appVersion = appOptions.Value.ApiVersion,
        message = "Check application logs folder for detailed logging"
    });
})
.WithName("LogsInfo")
.WithOpenApi();

logger.LogInformation("ServiceHub API started successfully");
app.Run();

// ========== MODELS ==========

namespace ServiceHub.Day17.Models
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

namespace ServiceHub.Day17.Options
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

// ========== REPOSITORIES ==========

namespace ServiceHub.Day17.Repositories
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

// ========== SERVICES ==========

namespace ServiceHub.Day17.Services
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
            "demo" => "Configured with Serilog logging",
            _ => "Unknown setting"
        };
    }

    public class ConsoleEmailService : IEmailService
    {
        private readonly ILogger<ConsoleEmailService> _logger;
        private readonly IOptions<EmailSettings> _emailOptions;

        public ConsoleEmailService(IOptions<EmailSettings> emailOptions, ILogger<ConsoleEmailService> logger)
        {
            _emailOptions = emailOptions;
            _logger = logger;
        }

        public Task SendAsync(string to, string subject, string message)
        {
            var settings = _emailOptions.Value;
            _logger.LogInformation("Sending email [{Provider}] to {Email} - Subject: {Subject}", settings.Provider, to, subject);
            return Task.CompletedTask;
        }
    }

    public class SmtpEmailService : IEmailService
    {
        private readonly ILogger<SmtpEmailService> _logger;
        private readonly IOptions<EmailSettings> _emailOptions;

        public SmtpEmailService(IOptions<EmailSettings> emailOptions, ILogger<SmtpEmailService> logger)
        {
            _emailOptions = emailOptions;
            _logger = logger;
        }

        public Task SendAsync(string to, string subject, string message)
        {
            var settings = _emailOptions.Value;
            _logger.LogInformation("Sending SMTP email to {Email} via {SmtpServer}:{SmtpPort} - Subject: {Subject}", to, settings.SmtpServer, settings.SmtpPort, subject);
            return Task.CompletedTask;
        }
    }

    public class CustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ICustomerRepository repository, ILogger<CustomerService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Customer> CreateAsync(string name, string email)
        {
            _logger.LogDebug("Creating customer: {Name} ({Email})", name, email);
            var customer = new Customer { Name = name, Email = email };
            await _repository.AddAsync(customer);
            _logger.LogInformation("Customer created: {CustomerId}", customer.Id);
            return customer;
        }

        public async Task<Customer?> GetAsync(int id)
        {
            _logger.LogDebug("Getting customer: {CustomerId}", id);
            return await _repository.GetAsync(id);
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            _logger.LogDebug("Getting all customers");
            return await _repository.GetAllAsync();
        }
    }

    public class WorkOrderService
    {
        private readonly IWorkOrderRepository _repository;
        private readonly ILogger<WorkOrderService> _logger;

        public WorkOrderService(IWorkOrderRepository repository, ILogger<WorkOrderService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<WorkOrder> CreateAsync(int customerId, string description)
        {
            _logger.LogDebug("Creating work order for customer {CustomerId}: {Description}", customerId, description);
            var order = new WorkOrder { CustomerId = customerId, Description = description };
            await _repository.AddAsync(order);
            return order;
        }

        public async Task<List<WorkOrder>> GetAllAsync()
        {
            _logger.LogDebug("Getting all work orders");
            return await _repository.GetAllAsync();
        }
    }

    public class OrderService
    {
        private readonly ICustomerRepository _customers;
        private readonly IWorkOrderRepository _orders;
        private readonly IEmailService _email;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            ICustomerRepository customers,
            IWorkOrderRepository orders,
            IEmailService email,
            ILogger<OrderService> logger)
        {
            _customers = customers;
            _orders = orders;
            _email = email;
            _logger = logger;
        }

        public async Task<WorkOrder> CreateOrderAsync(int customerId, string description)
        {
            _logger.LogInformation("Creating order for customer {CustomerId}: {Description}", customerId, description);
            
            var customer = await _customers.GetAsync(customerId);
            if (customer == null)
            {
                _logger.LogError("Customer {CustomerId} not found", customerId);
                throw new InvalidOperationException($"Customer {customerId} not found");
            }

            var order = new WorkOrder { CustomerId = customerId, Description = description };
            await _orders.AddAsync(order);

            _logger.LogDebug("Sending notification email to {Email}", customer.Email);
            await _email.SendAsync(
                customer.Email,
                "New Work Order Created",
                $"Your work order #{order.Id} has been created: {description}"
            );

            _logger.LogInformation("Order created successfully: {OrderId}", order.Id);
            return order;
        }
    }
}

// ========== DTOs ==========

public record CreateCustomerRequest(string Name, string Email);
public record CreateWorkOrderRequest(int CustomerId, string Description);
