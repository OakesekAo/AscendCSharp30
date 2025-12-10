// Day 19 - Clean Architecture (Complete)
using Microsoft.Extensions.Options;
using Serilog;
var builder = WebApplicationBuilder.CreateBuilder(args);
builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration).Enrich.FromLogContext().Enrich.WithMachineName().Enrich.WithThreadId());
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IValidationService, ValidationService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<WorkOrderService>();
builder.Services.AddHttpClient<IExternalWeatherService, ExternalWeatherService>().ConfigureHttpClient(c => c.Timeout = TimeSpan.FromSeconds(10));
if (builder.Environment.IsProduction()) builder.Services.AddScoped<IEmailService, SmtpEmailService>();
else builder.Services.AddScoped<IEmailService, ConsoleEmailService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
var logger = app.Services.GetRequiredService<ILogger<Program>>();
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
app.UseHttpsRedirection();
app.MapGet("/health", () => Results.Ok(new { status = "ok", architecture = "clean" })).WithName("Health").WithOpenApi();
app.MapGet("/customers", async (CustomerService s) => Results.Ok(await s.GetAllAsync())).WithName("GetCustomers").WithOpenApi();
app.MapGet("/customers/{id}", async (int id, CustomerService s) => { var c = await s.GetAsync(id); return c != null ? Results.Ok(c) : Results.NotFound(); }).WithName("GetCustomerById").WithOpenApi();
app.MapPost("/customers", async (CreateCustomerRequest r, CustomerService s, IValidationService v) => { var e = v.ValidateCustomer(r); if (e.Any()) return Results.BadRequest(e); return Results.Created($"/customers/{(await s.CreateAsync(r.Name, r.Email)).Id}", await s.GetAsync((await s.CreateAsync(r.Name, r.Email)).Id)); }).WithName("CreateCustomer").WithOpenApi();
app.MapGet("/workorders", async (WorkOrderService s) => Results.Ok(await s.GetAllAsync())).WithName("GetWorkOrders").WithOpenApi();
app.MapPost("/workorders", async (CreateWorkOrderRequest r, CustomerService cs, WorkOrderService ws) => { var c = await cs.GetAsync(r.CustomerId); if (c == null) return Results.BadRequest("Customer not found"); var o = await ws.CreateAsync(r.CustomerId, r.Description); return Results.Created($"/workorders/{o.Id}", o); }).WithName("CreateWorkOrder").WithOpenApi();
logger.LogInformation("ServiceHub API started");
app.Run();
public record Customer(int Id, string Name, string Email);
public record WorkOrder(int Id, int CustomerId, string Description, string Status);
public record Weather(string City, string Condition, int Temperature);
public record EmailSettings { public string Provider { get; set; } = "Console"; }
public interface ICustomerRepository { Task<Customer?> GetAsync(int id); Task<List<Customer>> GetAllAsync(); Task AddAsync(Customer c); }
public interface IWorkOrderRepository { Task<WorkOrder?> GetAsync(int id); Task<List<WorkOrder>> GetAllAsync(); Task AddAsync(WorkOrder o); }
public interface IValidationService { List<string> ValidateCustomer(CreateCustomerRequest r); }
public interface IEmailService { Task SendAsync(string to, string subject, string msg); }
public interface IExternalWeatherService { Task<Weather?> GetWeatherAsync(string city); }
public class CustomerRepository : ICustomerRepository
{
    private readonly List<Customer> _list = new() { new(1, "Alice", "a@ex.com"), new(2, "Bob", "b@ex.com") };
    public Task<Customer?> GetAsync(int id) => Task.FromResult(_list.FirstOrDefault(x => x.Id == id));
    public Task<List<Customer>> GetAllAsync() => Task.FromResult(_list);
    public Task AddAsync(Customer c) { _list.Add(c); return Task.CompletedTask; }
}
public class WorkOrderRepository : IWorkOrderRepository
{
    private readonly List<WorkOrder> _list = new() { new(1, 1, "Gutter Cleaning", "Scheduled") };
    public Task<WorkOrder?> GetAsync(int id) => Task.FromResult(_list.FirstOrDefault(x => x.Id == id));
    public Task<List<WorkOrder>> GetAllAsync() => Task.FromResult(_list);
    public Task AddAsync(WorkOrder o) { _list.Add(o); return Task.CompletedTask; }
}
public class ValidationService : IValidationService
{
    public List<string> ValidateCustomer(CreateCustomerRequest r)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(r.Name)) errors.Add("Name required");
        if (string.IsNullOrWhiteSpace(r.Email) || !r.Email.Contains("@")) errors.Add("Valid email required");
        return errors;
    }
}
public class ConsoleEmailService : IEmailService { public Task SendAsync(string to, string subject, string msg) => Task.CompletedTask; }
public class SmtpEmailService : IEmailService { public Task SendAsync(string to, string subject, string msg) => Task.CompletedTask; }
public class ExternalWeatherService : IExternalWeatherService
{
    private readonly HttpClient _http;
    public ExternalWeatherService(HttpClient http) => _http = http;
    public async Task<Weather?> GetWeatherAsync(string city) { await Task.Delay(100); return new Weather(city, "Sunny", 72); }
}
public class CustomerService
{
    private readonly ICustomerRepository _repo;
    public CustomerService(ICustomerRepository r) => _repo = r;
    public async Task<Customer> CreateAsync(string name, string email) { var c = new Customer(0, name, email); await _repo.AddAsync(c); return c; }
    public async Task<Customer?> GetAsync(int id) => await _repo.GetAsync(id);
    public async Task<List<Customer>> GetAllAsync() => await _repo.GetAllAsync();
}
public class WorkOrderService
{
    private readonly IWorkOrderRepository _repo;
    public WorkOrderService(IWorkOrderRepository r) => _repo = r;
    public async Task<WorkOrder> CreateAsync(int cid, string desc) { var o = new WorkOrder(0, cid, desc, "Scheduled"); await _repo.AddAsync(o); return o; }
    public async Task<List<WorkOrder>> GetAllAsync() => await _repo.GetAllAsync();
}
public record CreateCustomerRequest(string Name, string Email);
public record CreateWorkOrderRequest(int CustomerId, string Description);
