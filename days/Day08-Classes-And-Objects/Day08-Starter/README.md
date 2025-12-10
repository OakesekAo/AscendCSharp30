# Day 08 — Dependency Injection Foundations

## 🚀 Week 2: API Sprint Begins

**Maya's Message:**
> "Week 1 is done. You know C# now. This week, we're building the heart of ServiceHub — the API. Every feature needs to be flexible, testable, and maintainable. That's what DI gives us."

Today, you'll learn **Dependency Injection** — the foundation for professional, testable code and the pattern that powers all modern .NET applications.

**Important:** Starting today, we're building a **web API** (not console apps). You'll learn HTTP fundamentals while mastering DI.

---

## 🎯 Learning Objectives

1. **Understand the problem:** Tightly-coupled dependencies are hard to test and maintain
2. **Learn the solution:** Dependency Injection (DI) and interfaces
3. **Create an N-tier architecture:** Models → Repositories → Services → Endpoints
4. **Register services with DI:** Wire up the dependency container
5. **Build basic endpoints:** GET, POST with injected services
6. **Run a web API:** See it working in Swagger

---

## 📋 Prerequisites

Before you start:
- Days 01-07 complete (comfortable with classes and basic HTTP concepts)
- .NET 10 SDK installed
- Your editor open
- ~90 minutes

---

## Setup: Create Your Day 08 Web Project

```bash
mkdir Day08-DependencyInjection
cd Day08-DependencyInjection
dotnet new web
```

This creates an **ASP.NET Core web project** (not console).

---

## Step 1: The Problem Without DI

**Bad code (tightly-coupled):**

```csharp
class CustomerService
{
    // Creates its own repository - hard to test, hard to swap
    private CustomerRepository repository = new CustomerRepository();
    
    public void AddCustomer(string name)
    {
        repository.Save(name);
    }
}
```

**Problems:**
- ❌ Service creates its own dependency
- ❌ Can't use a fake/test version
- ❌ Hard to change implementation later
- ❌ Tightly coupled, not flexible

---

## Step 2: The Solution: Constructor Injection

**Good code (loosely-coupled):**

```csharp
interface ICustomerRepository
{
    Task<Customer?> GetAsync(int id);
    Task<List<Customer>> GetAllAsync();
}

class CustomerRepository : ICustomerRepository
{
    private List<Customer> _customers = new();
    
    public Task<Customer?> GetAsync(int id)
        => Task.FromResult(_customers.FirstOrDefault(c => c.Id == id));
    
    public Task<List<Customer>> GetAllAsync()
        => Task.FromResult(new List<Customer>(_customers));
}

// Service receives dependency in constructor
class CustomerService
{
    private readonly ICustomerRepository _repository;
    
    public CustomerService(ICustomerRepository repository)
    {
        _repository = repository;  // Injected!
    }
    
    public async Task<Customer?> GetAsync(int id)
        => await _repository.GetAsync(id);
    
    public async Task<List<Customer>> GetAllAsync()
        => await _repository.GetAllAsync();
}
```

**Benefits:**
- ✅ Service doesn't create repository
- ✅ Easy to inject fake version for tests
- ✅ Easy to swap implementations
- ✅ Loosely-coupled, flexible, testable

---

## Step 3: N-Tier Architecture (Professional Pattern)

This is the **structure you'll use all week:**

```
Models/                  (What data looks like)
├── Customer.cs
└── WorkOrder.cs

Repositories/           (How data is accessed)
├── ICustomerRepository.cs
└── CustomerRepository.cs

Services/              (Business logic)
├── ICustomerService.cs
└── CustomerService.cs

Program.cs            (Web API + DI wiring + endpoints)
```

**Each layer has a job:**
1. **Models** - Define entity shapes
2. **Repositories** - Abstract data access behind interfaces
3. **Services** - Business logic that uses repositories
4. **Endpoints** - HTTP handlers that use services
5. **DI Container** - Wires it all together

---

## Step 4: Build Your First Endpoint

**Register services in Program.cs:**

```csharp
var builder = WebApplicationBuilder.CreateBuilder(args);

// Register repositories and services
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<CustomerService>();

var app = builder.Build();

// Endpoints
app.MapGet("/customers", async (CustomerService service) =>
{
    var customers = await service.GetAllAsync();
    return Results.Ok(customers);
})
.WithName("GetCustomers")
.WithOpenApi();

app.Run();
```

**What happens:**
1. User requests `GET /customers`
2. DI container sees endpoint needs `CustomerService`
3. DI container sees `CustomerService` needs `ICustomerRepository`
4. DI container creates both and injects them
5. Endpoint calls service, returns response

---

## Step 5: Mini Challenge — Build ServiceHub API

**Create a web project with:**

1. **Models:**
   - `Customer` (Id, Name, Email)
   - `WorkOrder` (Id, CustomerId, Description, Status)

2. **Repositories:**
   - `ICustomerRepository` (GetAsync, GetAllAsync, AddAsync)
   - `IWorkOrderRepository` (GetAsync, GetAllAsync, AddAsync)
   - Implementations with in-memory storage

3. **Services:**
   - `CustomerService` (uses ICustomerRepository)
   - `WorkOrderService` (uses IWorkOrderRepository)

4. **Endpoints in Program.cs:**
   - `GET /customers` - List all
   - `GET /customers/{id}` - Get one
   - `POST /customers` - Create
   - `GET /workorders` - List all
   - `GET /workorders/{id}` - Get one
   - `POST /workorders` - Create

5. **Add Swagger:**
   ```csharp
   builder.Services.AddEndpointsApiExplorer();
   builder.Services.AddSwaggerGen();
   ```

6. **Run and test:**
   ```bash
   dotnet run
   # Open https://localhost:5001/swagger
   ```

---

## 🔍 Compare to Complete Example

After building, compare your code to `Day08-Complete/`:
- Same structure?
- Same patterns?
- Same endpoints?

The **Complete example** shows the professional way to organize this code.

---

## ✅ Checklist

- [ ] Created web project (`dotnet new web`)
- [ ] Created Models/ folder with Customer, WorkOrder
- [ ] Created Repositories/ folder with interfaces and implementations
- [ ] Created Services/ folder with business logic
- [ ] Registered services in DI container
- [ ] Built at least 3 endpoints
- [ ] Tested with Swagger UI
- [ ] Code compiles and runs without errors
- [ ] Endpoints return correct data

---

## 🔗 Next Steps

Day 09: **Add DTOs** — Separate API contracts from domain models.

---

## 📚 Resources

- <a href="https://docs.microsoft.com/dotnet/csharp/" target="_blank">C# Documentation</a>
- <a href="https://github.com/OakesekAo/AscendCSharp30" target="_blank">AscendCSharp30 Repository</a>

---

## 🎬 Why This Matters for ServiceHub

This DI pattern is **the foundation for everything Week 2+:**

- **Day 08:** Learn DI foundations (today)
- **Day 09:** Add DTOs + organized endpoints
- **Day 10:** Add error handling
- **Day 11:** Convert to async (already done here!)
- **Day 12-14:** Add search, filtering, analytics
- **Week 3:** Replace repositories with EF Core
- **Week 4:** Inject authentication, logging, configuration

**Every day builds on this foundation.**

---

**You've got this.** See you on Day 09! 🚀

