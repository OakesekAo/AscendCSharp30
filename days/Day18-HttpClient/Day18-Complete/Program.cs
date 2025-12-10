using Microsoft.Extensions.Options;
using Serilog;
using ServiceHub.Day18.Services;
using ServiceHub.Day18.Repositories;
using ServiceHub.Day18.Models;
using ServiceHub.Day18.Options;

var builder = WebApplicationBuilder.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithThreadId());

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddTransient<IValidationService, ValidationService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddSingleton<IMemoryCache>(new SimpleMemoryCache());
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<WorkOrderService>();
builder.Services.AddScoped<OrderService>();

// HttpClient factory setup
builder.Services.AddHttpClient<IExternalWeatherService, ExternalWeatherService>()
    .ConfigureHttpClient(client => client.Timeout = TimeSpan.FromSeconds(10));

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
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Starting ServiceHub API with HttpClient");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/health", () => Results.Ok(new { status = "ok", version = "1.0" }))
    .WithName("Health").WithOpenApi();

app.MapGet("/customers", async (CustomerService service) =>
{
    var customers = await service.GetAllAsync();
    return Results.Ok(customers);
})
.WithName("GetCustomers").WithOpenApi();

app.MapGet("/customers/{id}", async (int id, CustomerService service) =>
{
    var customer = await service.GetAsync(id);
    return customer != null ? Results.Ok(customer) : Results.NotFound();
})
.WithName("GetCustomerById").WithOpenApi();

app.MapPost("/customers", async (CreateCustomerRequest request, CustomerService service, IValidationService validator) =>
{
    var errors = validator.ValidateCustomer(request);
    if (errors.Any()) return Results.BadRequest(new { errors });
    var customer = await service.CreateAsync(request.Name, request.Email);
    return Results.Created($"/customers/{customer.Id}", customer);
})
.WithName("CreateCustomer").WithOpenApi();

app.MapGet("/workorders", async (WorkOrderService service) =>
{
    var orders = await service.GetAllAsync();
    return Results.Ok(orders);
})
.WithName("GetWorkOrders").WithOpenApi();

app.MapPost("/workorders", async (CreateWorkOrderRequest request, OrderService orderService, IValidationService validator) =>
{
    var errors = validator.ValidateWorkOrder(request);
    if (errors.Any()) return Results.BadRequest(new { errors });
    try
    {
        var order = await orderService.CreateOrderAsync(request.CustomerId, request.Description);
        return Results.Created($"/workorders/{order.Id}", order);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to create work order");
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("CreateWorkOrder").WithOpenApi();

// External API endpoint
app.MapGet("/weather/{city}", async (string city, IExternalWeatherService weatherService) =>
{
    logger.LogInformation("Fetching weather for {City}", city);
    try
    {
        var weather = await weatherService.GetWeatherAsync(city);
        return weather != null ? Results.Ok(weather) : Results.NotFound();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to fetch weather");
        return Results.BadRequest(new { error = "Failed to fetch weather" });
    }
})
.WithName("GetWeather").WithOpenApi();

logger.LogInformation("ServiceHub API started");
app.Run();

namespace ServiceHub.Day18.Models
{
    public class Customer { public int Id { get; set; } public string Name { get; set; } = ""; public string Email { get; set; } = ""; }
    public class WorkOrder { public int Id { get; set; } public int CustomerId { get; set; } public string Description { get; set; } = ""; public string Status { get; set; } = "Scheduled"; }
    public class Weather { public string City { get; set; } = ""; public string Condition { get; set; } = ""; public int Temperature { get; set; } }
}

namespace ServiceHub.Day18.Options
{
    public class EmailSettings { public string Provider { get; set; } = "Console"; public string SmtpServer { get; set; } = ""; public int SmtpPort { get; set; } = 587; }
    public class DatabaseSettings { public string ConnectionString { get; set; } = ""; public string Provider { get; set; } = "InMemory"; }
    public class AppSettings { public string ApiVersion { get; set; } = "1.0.0"; public string AppName { get; set; } = "ServiceHub"; }
}

namespace ServiceHub.Day18.Repositories
{
    public interface ICustomerRepository { Task<Customer?> GetAsync(int id); Task<List<Customer>> GetAllAsync(); Task AddAsync(Customer customer); }
    public interface IWorkOrderRepository { Task<WorkOrder?> GetAsync(int id); Task<List<WorkOrder>> GetAllAsync(); Task AddAsync(WorkOrder order); }
    public interface IRepositoryFactory { ICustomerRepository GetCustomerRepository(); IWorkOrderRepository GetWorkOrderRepository(); }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly List<Customer> _customers = new();
        private int _nextId = 1;
        public CustomerRepository() { _customers.Add(new Customer { Id = _nextId++, Name = "Alice Johnson", Email = "alice@example.com" }); _customers.Add(new Customer { Id = _nextId++, Name = "Bob Smith", Email = "bob@example.com" }); }
        public Task<Customer?> GetAsync(int id) => Task.FromResult(_customers.FirstOrDefault(c => c.Id == id));
        public Task<List<Customer>> GetAllAsync() => Task.FromResult(new List<Customer>(_customers));
        public Task AddAsync(Customer customer) { customer.Id = _nextId++; _customers.Add(customer); return Task.CompletedTask; }
    }

    public class WorkOrderRepository : IWorkOrderRepository
    {
        private readonly List<WorkOrder> _orders = new();
        private int _nextId = 1;
        public WorkOrderRepository() { _orders.Add(new WorkOrder { Id = _nextId++, CustomerId = 1, Description = "Gutter Cleaning", Status = "Scheduled" }); }
        public Task<WorkOrder?> GetAsync(int id) => Task.FromResult(_orders.FirstOrDefault(o => o.Id == id));
        public Task<List<WorkOrder>> GetAllAsync() => Task.FromResult(new List<WorkOrder>(_orders));
        public Task AddAsync(WorkOrder order) { order.Id = _nextId++; _orders.Add(order); return Task.CompletedTask; }
    }

    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly ICustomerRepository _customers;
        private readonly IWorkOrderRepository _orders;
        public RepositoryFactory(ICustomerRepository customers, IWorkOrderRepository orders) { _customers = customers; _orders = orders; }
        public ICustomerRepository GetCustomerRepository() => _customers;
        public IWorkOrderRepository GetWorkOrderRepository() => _orders;
    }
}

namespace ServiceHub.Day18.Services
{
    public interface IValidationService { List<string> ValidateCustomer(CreateCustomerRequest request); List<string> ValidateWorkOrder(CreateWorkOrderRequest request); }
    public interface IEmailService { Task SendAsync(string to, string subject, string message); }
    public interface IMemoryCache { void Set<T>(string key, T value, TimeSpan? expiration = null); bool TryGetValue<T>(string key, out T? value); }
    public interface IExternalWeatherService { Task<Weather?> GetWeatherAsync(string city); }

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
            if (_cache.TryGetValue(key, out var obj)) { value = (T?)obj; return true; }
            value = default; return false;
        }
    }

    public class ConsoleEmailService : IEmailService
    {
        private readonly ILogger<ConsoleEmailService> _logger;
        public ConsoleEmailService(ILogger<ConsoleEmailService> logger) => _logger = logger;
        public Task SendAsync(string to, string subject, string message) { _logger.LogInformation("📧 Email to {Email}: {Subject}", to, subject); return Task.CompletedTask; }
    }

    public class SmtpEmailService : IEmailService
    {
        private readonly ILogger<SmtpEmailService> _logger;
        public SmtpEmailService(ILogger<SmtpEmailService> logger) => _logger = logger;
        public Task SendAsync(string to, string subject, string message) { _logger.LogInformation("📧 [SMTP] Email to {Email}", to); return Task.CompletedTask; }
    }

    public class ExternalWeatherService : IExternalWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExternalWeatherService> _logger;
        public ExternalWeatherService(HttpClient httpClient, ILogger<ExternalWeatherService> logger) { _httpClient = httpClient; _logger = logger; }
        public async Task<Weather?> GetWeatherAsync(string city)
        {
            _logger.LogInformation("Fetching weather for {City}", city);
            try
            {
                await Task.Delay(100);
                var weather = new Weather { City = city, Condition = "Sunny", Temperature = 72 };
                _logger.LogInformation("Weather fetched: {Temp}°", weather.Temperature);
                return weather;
            }
            catch (Exception ex) { _logger.LogError(ex, "Failed to fetch weather"); return null; }
        }
    }

    public class CustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly ILogger<CustomerService> _logger;
        public CustomerService(ICustomerRepository repository, ILogger<CustomerService> logger) { _repository = repository; _logger = logger; }
        public async Task<Customer> CreateAsync(string name, string email) { _logger.LogDebug("Creating customer: {Name}", name); var customer = new Customer { Name = name, Email = email }; await _repository.AddAsync(customer); return customer; }
        public async Task<Customer?> GetAsync(int id) => await _repository.GetAsync(id);
        public async Task<List<Customer>> GetAllAsync() => await _repository.GetAllAsync();
    }

    public class WorkOrderService
    {
        private readonly IWorkOrderRepository _repository;
        private readonly ILogger<WorkOrderService> _logger;
        public WorkOrderService(IWorkOrderRepository repository, ILogger<WorkOrderService> logger) { _repository = repository; _logger = logger; }
        public async Task<WorkOrder> CreateAsync(int customerId, string description) { _logger.LogDebug("Creating work order for customer {CustomerId}", customerId); var order = new WorkOrder { CustomerId = customerId, Description = description }; await _repository.AddAsync(order); return order; }
        public async Task<List<WorkOrder>> GetAllAsync() => await _repository.GetAllAsync();
    }

    public class OrderService
    {
        private readonly ICustomerRepository _customers;
        private readonly IWorkOrderRepository _orders;
        private readonly IEmailService _email;
        private readonly ILogger<OrderService> _logger;
        public OrderService(ICustomerRepository customers, IWorkOrderRepository orders, IEmailService email, ILogger<OrderService> logger) { _customers = customers; _orders = orders; _email = email; _logger = logger; }
        public async Task<WorkOrder> CreateOrderAsync(int customerId, string description)
        {
            _logger.LogInformation("Creating order for customer {CustomerId}", customerId);
            var customer = await _customers.GetAsync(customerId);
            if (customer == null) { _logger.LogError("Customer {CustomerId} not found", customerId); throw new InvalidOperationException($"Customer {customerId} not found"); }
            var order = new WorkOrder { CustomerId = customerId, Description = description };
            await _orders.AddAsync(order);
            await _email.SendAsync(customer.Email, "New Work Order", $"Order #{order.Id} created");
            return order;
        }
    }
}

public record CreateCustomerRequest(string Name, string Email);
public record CreateWorkOrderRequest(int CustomerId, string Description);
