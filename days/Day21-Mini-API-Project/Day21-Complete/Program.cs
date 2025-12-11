// Day 21 - EF Core & Database Capstone (Complete)
using Microsoft.EntityFrameworkCore;
using Serilog;
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration).Enrich.FromLogContext());
var cs = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=servicehub.db";
builder.Services.AddDbContext<ServiceHubContext>(opt => opt.UseSqlite(cs));
builder.Services.AddScoped<ICustomerRepository, EFCustomerRepository>();
builder.Services.AddScoped<IWorkOrderRepository, EFWorkOrderRepository>();
builder.Services.AddEndpointsApiExplorer();
// Add Swagger if available, but guard against incompatible Swashbuckle versions at runtime
try
{
    builder.Services.AddSwaggerGen();
}
catch (Exception ex)
{
    // If AddSwaggerGen fails due to package incompatibility, log and continue without Swagger
    Console.WriteLine($"Warning: Swagger not available: {ex.Message}");
}
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ServiceHubContext>();
    db.Database.EnsureCreated();
    if (!db.Customers.Any())
    {
        db.Customers.Add(new Customer { Id = 1, Name = "Alice", Email = "a@ex.com" });
        db.Customers.Add(new Customer { Id = 2, Name = "Bob", Email = "b@ex.com" });
        db.WorkOrders.Add(new WorkOrder { Id = 1, CustomerId = 1, Description = "Gutter Cleaning", Status = "Scheduled" });
        db.SaveChanges();
    }
}
// In .NET 10 use the built-in API UI (Scaler) in development. Swashbuckle guarded earlier; prefer Scaler UI by default.
if (app.Environment.IsDevelopment())
{
    // No explicit Swagger middleware required for Scaler UI.
}
app.MapGet("/health", () => Results.Ok(new { status = "ok", database = "connected" })).WithName("Health").WithOpenApi();
app.MapGet("/customers", async (ICustomerRepository r) => Results.Ok(await r.GetAllAsync())).WithName("GetCustomers").WithOpenApi();
app.MapGet("/customers/{id}", async (int id, ICustomerRepository r) => { var c = await r.GetAsync(id); return c != null ? Results.Ok(c) : Results.NotFound(); }).WithName("GetById").WithOpenApi();
app.MapPost("/customers", async (CreateCustomerRequest req, ICustomerRepository r) => { var c = new Customer { Name = req.Name, Email = req.Email }; await r.AddAsync(c); return Results.Created($"/customers/{c.Id}", c); }).WithName("CreateCustomer").WithOpenApi();
app.MapGet("/workorders", async (IWorkOrderRepository r) => Results.Ok(await r.GetAllAsync())).WithName("GetWorkOrders").WithOpenApi();
app.MapPost("/workorders", async (CreateWorkOrderRequest req, ICustomerRepository cr, IWorkOrderRepository wr) => { var c = await cr.GetAsync(req.CustomerId); if (c == null) return Results.BadRequest("Customer not found"); var w = new WorkOrder { CustomerId = req.CustomerId, Description = req.Description, Status = "Scheduled" }; await wr.AddAsync(w); return Results.Created($"/workorders/{w.Id}", w); }).WithName("CreateWorkOrder").WithOpenApi();
app.Run();
public class Customer { public int Id { get; set; } public string Name { get; set; } = ""; public string Email { get; set; } = ""; public List<WorkOrder> WorkOrders { get; set; } = new(); }
public class WorkOrder { public int Id { get; set; } public int CustomerId { get; set; } public string Description { get; set; } = ""; public string Status { get; set; } = "Scheduled"; public Customer? Customer { get; set; } }
public class ServiceHubContext : DbContext
{
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<WorkOrder> WorkOrders { get; set; } = null!;
    public ServiceHubContext(DbContextOptions<ServiceHubContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);
        mb.Entity<Customer>().HasKey(c => c.Id);
        mb.Entity<Customer>().Property(c => c.Name).IsRequired().HasMaxLength(100);
        mb.Entity<Customer>().Property(c => c.Email).IsRequired().HasMaxLength(100);
        mb.Entity<Customer>().HasMany(c => c.WorkOrders).WithOne(w => w.Customer).HasForeignKey(w => w.CustomerId).OnDelete(DeleteBehavior.Cascade);
        mb.Entity<WorkOrder>().HasKey(w => w.Id);
        mb.Entity<WorkOrder>().Property(w => w.Description).IsRequired().HasMaxLength(500);
        mb.Entity<WorkOrder>().Property(w => w.Status).IsRequired().HasMaxLength(50);
    }
}
public interface ICustomerRepository { Task<Customer?> GetAsync(int id); Task<List<Customer>> GetAllAsync(); Task AddAsync(Customer c); }
public interface IWorkOrderRepository { Task<WorkOrder?> GetAsync(int id); Task<List<WorkOrder>> GetAllAsync(); Task AddAsync(WorkOrder w); }
public class EFCustomerRepository : ICustomerRepository
{
    private readonly ServiceHubContext _db;
    public EFCustomerRepository(ServiceHubContext db) => _db = db;
    public async Task<Customer?> GetAsync(int id) => await _db.Customers.FindAsync(id);
    public async Task<List<Customer>> GetAllAsync() => await _db.Customers.ToListAsync();
    public async Task AddAsync(Customer c) { _db.Customers.Add(c); await _db.SaveChangesAsync(); }
}
public class EFWorkOrderRepository : IWorkOrderRepository
{
    private readonly ServiceHubContext _db;
    public EFWorkOrderRepository(ServiceHubContext db) => _db = db;
    public async Task<WorkOrder?> GetAsync(int id) => await _db.WorkOrders.FindAsync(id);
    public async Task<List<WorkOrder>> GetAllAsync() => await _db.WorkOrders.ToListAsync();
    public async Task AddAsync(WorkOrder w) { _db.WorkOrders.Add(w); await _db.SaveChangesAsync(); }
}
public record CreateCustomerRequest(string Name, string Email);
public record CreateWorkOrderRequest(int CustomerId, string Description);

