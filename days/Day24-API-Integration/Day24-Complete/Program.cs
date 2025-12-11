// Day 24 - API Integration
using Microsoft.EntityFrameworkCore;
var builder = WebApplicationBuilder.CreateBuilder(args);
var cs = "Data Source=servicehub.db";
builder.Services.AddDbContext<Db>(o => o.UseSqlite(cs));
builder.Services.AddScoped<ICustRepo, EFCustRepo>();
builder.Services.AddScoped<IOrdRepo, EFOrdRepo>();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
using (var s = app.Services.CreateScope()) { var db = s.ServiceProvider.GetRequiredService<Db>(); db.Database.EnsureCreated(); }
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
app.MapGet("/api/customers", async (ICustRepo r) => Results.Ok(await r.GetAllAsync())).WithOpenApi();
app.MapPost("/api/customers", async (C c, ICustRepo r) => { await r.AddAsync(c); return Results.Created("", c); }).WithOpenApi();
app.MapGet("/api/orders", async (IOrdRepo r) => Results.Ok(await r.GetAllWithCustAsync())).WithOpenApi();
app.MapPost("/api/orders", async (O o, IOrdRepo r) => { await r.AddAsync(o); return Results.Created("", o); }).WithOpenApi();
app.Run();

public class C { public int Id { get; set; } public string N { get; set; } = ""; public List<O> Os { get; set; } = new(); }
public class O { public int Id { get; set; } public int Cid { get; set; } public string D { get; set; } = ""; public C? C { get; set; } }
public class Db : DbContext
{
    public DbSet<C> Cs { get; set; } = null!;
    public DbSet<O> Os { get; set; } = null!;
    public Db(DbContextOptions<Db> o) : base(o) { }
}

public interface ICustRepo { Task<List<C>> GetAllAsync(); Task AddAsync(C c); }
public interface IOrdRepo { Task<List<O>> GetAllWithCustAsync(); Task AddAsync(O o); }

public class EFCustRepo : ICustRepo
{
    private readonly Db _db;
    public EFCustRepo(Db db) => _db = db;
    public async Task<List<C>> GetAllAsync() => await _db.Cs.ToListAsync();
    public async Task AddAsync(C c) { _db.Cs.Add(c); await _db.SaveChangesAsync(); }
}

public class EFOrdRepo : IOrdRepo
{
    private readonly Db _db;
    public EFOrdRepo(Db db) => _db = db;
    public async Task<List<O>> GetAllWithCustAsync() => await _db.Os.Include(o => o.C).ToListAsync();
    public async Task AddAsync(O o) { _db.Os.Add(o); await _db.SaveChangesAsync(); }
}
