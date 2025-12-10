# Day 15 — Advanced Dependency Injection (Complete)

## 🎯 Building on Week 2

This is the **complete, working implementation** of Day 15: **mastering advanced DI patterns**.

**Key difference from Day 14:**
- Day 14: Basic Scoped services (Repositories, Services)
- Day 15: Transient, Scoped, Singleton lifetimes + factories + service composition

---

## 🏗️ Advanced DI Architecture

```
Day 15 Complete/
├── Services/
│   ├── IValidationService (Transient - stateless)
│   ├── ValidationService
│   ├── IEmailService (interface)
│   ├── ConsoleEmailService (dev)
│   ├── SmtpEmailService (production)
│   ├── IMemoryCache (Singleton - expensive to create)
│   ├── SimpleMemoryCache
│   ├── IConfigurationService (Singleton)
│   ├── ConfigurationService
│   ├── CustomerService (Scoped)
│   ├── WorkOrderService (Scoped)
│   └── OrderService (Service hierarchy - depends on repos + email)
│
├── Repositories/
│   ├── ICustomerRepository (Scoped)
│   ├── IWorkOrderRepository (Scoped)
│   ├── CustomerRepository
│   ├── WorkOrderRepository
│   ├── IRepositoryFactory (Factory pattern)
│   └── RepositoryFactory
│
├── Models/
│   ├── Customer.cs
│   └── WorkOrder.cs
│
└── Program.cs (DI wiring + endpoints)
```

---

## 🚀 Run This Code

### Prerequisites
- .NET 10 SDK installed
- Terminal open in `days/Day15-Dependency-Injection/Day15-Complete/`

### Run It
```bash
dotnet run
```

Open browser to: **https://localhost:5001/swagger/index.html**

---

## 📝 Key Concepts

### 1. DI Lifetimes

**Transient** - New instance every time
```csharp
builder.Services.AddTransient<IValidationService, ValidationService>();

// Every request creates new ValidationService
// Good for: Stateless, lightweight services
// Bad for: Expensive operations, state that should persist
```

**Scoped** - One per request
```csharp
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// One instance per HTTP request
// Good for: Repositories, DbContext, per-request state
// Bad for: Singletons, application-wide state
```

**Singleton** - One for entire application lifetime
```csharp
builder.Services.AddSingleton<IMemoryCache, SimpleMemoryCache>();

// Same instance for entire app life
// Good for: Configuration, caches, expensive resources
// Bad for: Stateful services, not thread-safe by default
```

### 2. Service Hierarchy (Composition)

```csharp
public class OrderService
{
    private readonly ICustomerRepository _customers;
    private readonly IWorkOrderRepository _orders;
    private readonly IEmailService _email;
    
    // Constructor receives all dependencies
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
        // Service uses multiple injected services
        var customer = await _customers.GetAsync(customerId);
        var order = await _orders.CreateAsync(customerId, description);
        await _email.SendAsync(customer.Email, "Order created");
        return order;
    }
}
```

**Benefits:**
- ✅ Services depend on abstractions (interfaces)
- ✅ Easy to compose complex behavior
- ✅ Easy to mock/test
- ✅ Clear dependencies

### 3. Factory Pattern

```csharp
// Instead of:
builder.Services.AddScoped<IRepository, Repository>();

// Use factory for complex initialization:
builder.Services.AddScoped<IRepositoryFactory>(serviceProvider =>
    new RepositoryFactory(
        serviceProvider.GetRequiredService<ICustomerRepository>(),
        serviceProvider.GetRequiredService<IWorkOrderRepository>()
    )
);
```

**Benefits:**
- ✅ Complex object creation logic
- ✅ Access to DI container during creation
- ✅ Conditional object creation

### 4. Conditional Registration

```csharp
// Different service based on environment
if (builder.Environment.IsProduction())
    builder.Services.AddScoped<IEmailService, SmtpEmailService>();
else
    builder.Services.AddScoped<IEmailService, ConsoleEmailService>();
```

**Benefits:**
- ✅ Dev/prod differences without code changes
- ✅ Flexible configuration
- ✅ Easy to swap implementations

---

## ✅ Endpoints Available

```
GET    /health               Health check (shows DI lifetimes)
GET    /customers            List all customers
GET    /customers/{id}       Get one customer
POST   /customers            Create customer (with validation)

GET    /workorders           List all work orders
POST   /workorders           Create (with email notification)

GET    /di-info              Show DI configuration info
```

---

## 📊 Advanced DI Features Demonstrated

| Feature | Where | Purpose |
|---------|-------|---------|
| **Transient** | ValidationService | Stateless, lightweight |
| **Scoped** | Repositories, Services | Per-request consistency |
| **Singleton** | Cache, Configuration | Application lifetime |
| **Factory** | RepositoryFactory | Complex initialization |
| **Conditional** | EmailService | Env-based selection |
| **Composition** | OrderService | Multiple dependencies |

---

## 🎯 What to Notice

1. **Lifetimes matter:** Transient, Scoped, Singleton each serve a purpose
2. **No `new` keyword:** DI container handles all creation
3. **Service hierarchy:** OrderService depends on multiple repos
4. **Factory pattern:** Complex objects created via factory
5. **Conditional registration:** Different implementations per environment
6. **Email service:** Sends notifications when orders created (side effect)

---

## 🔄 Data Flow Example

**User calls: `POST /workorders`**

```
Request with validation (Transient ValidationService created)
    ↓
OrderService (Scoped, receives Repos + Email)
    ↓
Gets Customer (Scoped CustomerRepository)
    ↓
Creates WorkOrder (Scoped WorkOrderRepository)
    ↓
Sends Email (Environment-specific EmailService)
    ↓
Returns Response
```

**Key:** Every layer uses injected dependencies, no direct instantiation.

---

## 📊 Seed Data

**Customers:**
- Alice Johnson
- Bob Smith

**Work Orders:**
- Gutter Cleaning (for Alice)

---

## 🎓 Professional Patterns Demonstrated

- ✅ **DI Lifetimes:** Proper lifetime selection
- ✅ **Service Composition:** Services depending on services
- ✅ **Factory Pattern:** Complex object creation
- ✅ **Conditional Registration:** Environment-based configuration
- ✅ **Transient Stateless:** Validation service
- ✅ **Scoped Per-Request:** Repositories and services
- ✅ **Singleton Expensive:** Cache and config
- ✅ **Email Notifications:** Side effects from services

---

## 🚀 Next: Day 16

Day 16 will **build on this code** by:
- Keeping this DI setup
- Adding Options pattern for configuration
- Reading from appsettings.json
- Environment-specific settings
- Secure credential management

**The DI foundation gets even more powerful.**

---

## 📖 Comparison: Day 14 vs Day 15

| Aspect | Day 14 | Day 15 |
|--------|--------|--------|
| **Lifetimes** | Only Scoped | Transient, Scoped, Singleton |
| **Services** | Independent | Hierarchical (OrderService) |
| **Factory** | None | Factory pattern |
| **Conditional** | None | Env-based service selection |
| **Email** | None | Email service (Console/SMTP) |
| **Caching** | None | Singleton memory cache |
| **Validation** | None | Transient validator |

---

**Ready to run?** `dotnet run` then visit `https://localhost:5001/swagger` 🚀

**This is enterprise-level DI configuration!**


