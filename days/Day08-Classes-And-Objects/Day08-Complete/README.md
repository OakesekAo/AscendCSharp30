# Day 08 — Dependency Injection Foundations (Complete)

## 🎯 What This Code Shows

This is the **complete, working implementation** of Day 08's concept: **building a web API with proper Dependency Injection**.

This is **the foundation** for Days 09-14. Every day will build on this code by adding more features.

---

## 🏗️ Architecture (Foundation for Week 2)

```
Day08-Complete/
├── Models/                          Domain entities
│   ├── Customer.cs
│   └── WorkOrder.cs
│
├── Repositories/                    Data access layer
│   ├── ICustomerRepository.cs       Interface
│   ├── IWorkOrderRepository.cs      Interface
│   ├── CustomerRepository.cs        Implementation
│   └── WorkOrderRepository.cs       Implementation
│
├── Services/                        Business logic layer
│   ├── CustomerService.cs           Uses ICustomerRepository
│   └── WorkOrderService.cs          Uses IWorkOrderRepository
│
├── Program.cs                       Web API bootstrap + endpoints
└── Day08-Complete.csproj           Web project file
```

**This is the N-tier architecture** you'll see in professional APIs.

---

## 🚀 Run This Code

### Prerequisites
- .NET 10 SDK installed
- Terminal open in `days/Day08-Classes-And-Objects/Day08-Complete/`

### Run It
```bash
dotnet run
```

You'll see:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
      Now listening on: http://localhost:5000
```

### Test It
Open browser to: **https://localhost:5001/swagger/index.html**

Or with curl:
```bash
curl -k https://localhost:5001/customers
```

---

## 📝 Key Concepts

### 1. Models (Domain Layer)
```csharp
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
}
```
**What it is:** Represents real-world entities
**Why it matters:** Single source of truth for data shape

### 2. Repositories (Data Access Layer)
```csharp
public interface ICustomerRepository
{
    Task AddAsync(Customer customer);
    Task<Customer?> GetAsync(int id);
    Task<List<Customer>> GetAllAsync();
}

public class CustomerRepository : ICustomerRepository
{
    private readonly List<Customer> _customers = new();
    
    public Task AddAsync(Customer customer)
    {
        customer.Id = _customers.Count + 1;
        _customers.Add(customer);
        return Task.CompletedTask;
    }
    // ...
}
```
**What it is:** Abstract data access behind an interface
**Why it matters:** Easy to swap real DB for tests, or change DB provider later

### 3. Services (Business Logic Layer)
```csharp
public class CustomerService
{
    private readonly ICustomerRepository _repository;
    
    public CustomerService(ICustomerRepository repository)  // Constructor injection
    {
        _repository = repository;
    }
    
    public async Task<Customer> CreateAsync(string name, string email)
    {
        var customer = new Customer { Name = name, Email = email };
        await _repository.AddAsync(customer);
        return customer;
    }
}
```
**What it is:** Orchestrates repositories and business rules
**Why it matters:** Keeps API endpoints clean, logic reusable

### 4. Endpoints (Presentation Layer)
```csharp
app.MapGet("/customers", async (CustomerService service) =>
{
    var customers = await service.GetAllAsync();
    return Results.Ok(customers);
})
.WithName("GetCustomers")
.WithOpenApi();
```
**What it is:** HTTP handlers that accept requests and return responses
**Why it matters:** DI injects services automatically

### 5. Dependency Injection (The Glue)
```csharp
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<CustomerService>();
```
**What it is:** Registers services so DI container can wire them
**Why it matters:** Automatic injection, loose coupling, testable code

---

## 🔄 Data Flow Example

**User calls: `GET /customers`**

1. **Endpoint** receives the request
2. **Endpoint** needs `CustomerService` → DI container provides it
3. **Service** needs `ICustomerRepository` → DI container provided it in constructor
4. **Service** calls `repository.GetAllAsync()`
5. **Repository** returns list from memory
6. **Service** returns to endpoint
7. **Endpoint** returns 200 OK with JSON

**The key: Service didn't create the repository. It was injected.**

---

## ✅ Endpoints Available

```
GET    /health                 Health check
GET    /customers              List all
GET    /customers/{id}         Get one
POST   /customers              Create

GET    /workorders             List all
GET    /workorders/{id}        Get one
POST   /workorders             Create
```

---

## 📊 Seed Data

Pre-loaded on startup:

**Customers:**
- Alice Johnson (alice@example.com)
- Bob Smith (bob@example.com)
- Charlie Brown (charlie@example.com)

**Work Orders:**
- Gutter Cleaning (for Alice, Scheduled)
- Lawn Mowing (for Bob, Scheduled)
- Window Washing (for Alice, InProgress)

---

## 🎯 What to Notice

1. **No `new` keyword** in services - DI provides dependencies
2. **Interfaces** define contracts - implementations can change
3. **Async/await** ready - repositories return `Task<T>`
4. **Layered architecture** - each layer has a job
5. **Testable** - can mock ICustomerRepository in tests

---

## 🚀 Next: Day 09

Day 09 will **build on this code** by:
- Keeping Models, Repositories, Services
- Adding DTOs (request/response models)
- Organizing endpoints into separate files
- Adding more complex endpoint logic

**The foundation is here. Days 09-14 add features.**

---

## 📖 Understanding the Pattern

This is how **professional .NET applications** are structured:

1. **Models** - What data looks like
2. **Repositories** - How data is accessed (abstract behind interface)
3. **Services** - Business logic that uses repositories
4. **Endpoints** - HTTP handlers that use services
5. **DI Container** - Wires it all together

**You're learning enterprise patterns from Day 08 onwards.**

---

**Ready to run?** `dotnet run` then visit `https://localhost:5001/swagger` 🚀

