// Day 23 - Advanced EF Core
using Microsoft.EntityFrameworkCore;
var builder = WebApplicationBuilder.CreateBuilder(args);
var cs = "Data Source=servicehub.db";
builder.Services.AddDbContext<AppDb>(o => o.UseSqlite(cs));
builder.Services.AddScoped<ICustomerRepo, EFCustomerRepo>();
builder.Services.AddScoped<IOrderRepo, EFOrderRepo>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
using (var s = app.Services.CreateScope()) { var db = s.ServiceProvider.GetRequiredService<AppDb>(); db.Database.EnsureCreated(); }
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
app.MapGet("/customers", async (ICustomerRepo r) => Results.Ok(await r.GetAllAsync())).WithOpenApi();
app.MapGet("/customers/{id}", async (int id, ICustomerRepo r) => { var c = await r.GetAsync(id); return c != null ? Results.Ok(c) : Results.NotFound(); }).WithOpenApi();
app.MapPost("/customers", async (Cust r, ICustomerRepo repo) => { await repo.AddAsync(r); return Results.Created("", r); }).WithOpenApi();
app.MapGet("/orders", async (IOrderRepo r) => Results.Ok(await r.GetAllWithCustomersAsync())).WithOpenApi();
app.MapPost("/orders", async (Ord o, IOrderRepo r) => { await r.AddAsync(o); return Results.Created("", o); }).WithOpenApi();
app.Run();

public class Cust { public int Id { get; set; } public string Name { get; set; } = ""; public List<Ord> Orders { get; set; } = new(); }
public class Ord { public int Id { get; set; } public int CustId { get; set; } public string Desc { get; set; } = ""; public Cust? Cust { get; set; } }
public class AppDb : DbContext
{
    public DbSet<Cust> Customers { get; set; } = null!;
    public DbSet<Ord> Orders { get; set; } = null!;
    public AppDb(DbContextOptions<AppDb> opts) : base(opts) { }
    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Cust>().HasKey(c => c.Id);
        mb.Entity<Cust>().HasMany(c => c.Orders).WithOne(o => o.Cust).HasForeignKey(o => o.CustId);
        mb.Entity<Ord>().HasKey(o => o.Id);
    }
}

public interface ICustomerRepo { Task<Cust?> GetAsync(int id); Task<List<Cust>> GetAllAsync(); Task AddAsync(Cust c); }
public interface IOrderRepo { Task<Ord?> GetAsync(int id); Task<List<Ord>> GetAllAsync(); Task<List<Ord>> GetAllWithCustomersAsync(); Task AddAsync(Ord o); }

public class EFCustomerRepo : ICustomerRepo
{
    private readonly AppDb _db;
    public EFCustomerRepo(AppDb db) => _db = db;
    public async Task<Cust?> GetAsync(int id) => await _db.Customers.FindAsync(id);
    public async Task<List<Cust>> GetAllAsync() => await _db.Customers.ToListAsync();
    public async Task AddAsync(Cust c) { _db.Customers.Add(c); await _db.SaveChangesAsync(); }
}

public class EFOrderRepo : IOrderRepo
{
    private readonly AppDb _db;
    public EFOrderRepo(AppDb db) => _db = db;
    public async Task<Ord?> GetAsync(int id) => await _db.Orders.FindAsync(id);
    public async Task<List<Ord>> GetAllAsync() => await _db.Orders.ToListAsync();
    public async Task<List<Ord>> GetAllWithCustomersAsync() => await _db.Orders.Include(o => o.Cust).ToListAsync();
    public async Task AddAsync(Ord o) { _db.Orders.Add(o); await _db.SaveChangesAsync(); }
}
