// Day 20 - Advanced LINQ (Complete)
using Serilog;
var builder = WebApplicationBuilder.CreateBuilder(args);
builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration).Enrich.FromLogContext());
builder.Services.AddScoped<IQueryService, QueryService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
var svc = app.Services.GetRequiredService<IQueryService>();
app.MapGet("/health", () => Results.Ok(new { status = "ok", feature = "advanced-linq" })).WithName("Health").WithOpenApi();
app.MapGet("/customers/advanced/count", async () => { var c = await svc.GetCustomerCountAsync(); return Results.Ok(new { count = c }); }).WithName("CustomerCount").WithOpenApi();
app.MapGet("/workorders/advanced/by-status", async () => { var g = await svc.GetWorkOrdersByStatusAsync(); return Results.Ok(g); }).WithName("WorkOrdersByStatus").WithOpenApi();
app.MapGet("/workorders/advanced/customer-stats", async () => { var s = await svc.GetCustomerWorkOrderStatsAsync(); return Results.Ok(s); }).WithName("CustomerStats").WithOpenApi();
app.MapGet("/customers/advanced/with-orders", async () => { var c = await svc.GetCustomersWithOrderCountAsync(); return Results.Ok(c); }).WithName("CustomersWithOrders").WithOpenApi();
app.Run();
public record Customer(int Id, string Name, string Email);
public record WorkOrder(int Id, int CustomerId, string Description, string Status);
public record CustomerOrderStat(int CustomerId, string CustomerName, int OrderCount);
public interface IQueryService
{
    Task<int> GetCustomerCountAsync();
    Task<Dictionary<string, List<WorkOrder>>> GetWorkOrdersByStatusAsync();
    Task<List<CustomerOrderStat>> GetCustomerWorkOrderStatsAsync();
    Task<List<(Customer, int)>> GetCustomersWithOrderCountAsync();
}
public class QueryService : IQueryService
{
    private readonly List<Customer> _customers = new() { new(1, "Alice", "a@ex.com"), new(2, "Bob", "b@ex.com"), new(3, "Charlie", "c@ex.com") };
    private readonly List<WorkOrder> _orders = new() 
    { 
        new(1, 1, "Gutter", "Scheduled"),
        new(2, 1, "Windows", "InProgress"),
        new(3, 2, "Lawn", "Scheduled"),
        new(4, 3, "Roof", "Completed")
    };
    public Task<int> GetCustomerCountAsync()
    {
        var count = _customers.Count();
        return Task.FromResult(count);
    }
    public Task<Dictionary<string, List<WorkOrder>>> GetWorkOrdersByStatusAsync()
    {
        var grouped = _orders
            .GroupBy(o => o.Status)
            .ToDictionary(g => g.Key, g => g.ToList());
        return Task.FromResult(grouped);
    }
    public Task<List<CustomerOrderStat>> GetCustomerWorkOrderStatsAsync()
    {
        var stats = _customers
            .GroupJoin(_orders, c => c.Id, o => o.CustomerId, (c, orders) => new CustomerOrderStat(c.Id, c.Name, orders.Count()))
            .ToList();
        return Task.FromResult(stats);
    }
    public Task<List<(Customer, int)>> GetCustomersWithOrderCountAsync()
    {
        var result = _customers
            .Select(c => (c, _orders.Count(o => o.CustomerId == c.Id)))
            .ToList();
        return Task.FromResult(result);
    }
}

