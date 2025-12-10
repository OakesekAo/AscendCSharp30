# Day 15 — Advanced Dependency Injection Patterns

## 🚀 Week 3: Scaling Your Architecture

**Maya's Message:**
> "Your API works great. But as we add more features, dependency management gets complex. Today you'll master advanced DI patterns that make systems scalable and testable."

Today, you'll learn **advanced DI patterns**: lifetimes (Scoped, Transient, Singleton), factory patterns, and service hierarchies.

---

## 🎯 Learning Objectives

1. **Understand DI lifetimes:** When and why to use Scoped, Transient, Singleton
2. **Master factory patterns:** Creating complex objects with DI
3. **Build service hierarchies:** Services that depend on services
4. **Use decorators:** Add behavior without modifying code
5. **Configure advanced DI:** Factory methods, conditional registration

---

## 📋 Prerequisites

Before you start:
- Days 01-14 complete
- Comfortable with DI from Day 08-14
- Understand repository and service patterns
- ~90 minutes

---

## What Changed Since Day 14

Day 14 had:
```
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<CustomerService>();
```

Day 15 adds:
```
// Lifetimes (new)
builder.Services.AddSingleton<IConfiguration>();        // Once for app
builder.Services.AddScoped<UnitOfWork>();               // Once per request
builder.Services.AddTransient<ValidationService>();     // Every time

// Factories (new)
builder.Services.AddScoped<IRepositoryFactory>(sp =>
    new RepositoryFactory(sp.GetRequiredService<DbContext>()));

// Decorators (new)
builder.Services.Decorate<ICustomerRepository, CachedCustomerRepository>();

// Conditional registration (new)
if (environment.IsProduction())
    builder.Services.AddScoped<IEmailService, SmtpEmailService>();
else
    builder.Services.AddScoped<IEmailService, ConsoleEmailService>();
```

---

## Step 1: Understand DI Lifetimes

### **Transient** - New instance every time
```csharp
builder.Services.AddTransient<IValidator, CustomerValidator>();

// Usage:
var validator1 = serviceProvider.GetService<IValidator>();
var validator2 = serviceProvider.GetService<IValidator>();
// validator1 != validator2 (different instances)
```

**When to use:**
- ✅ Stateless services
- ✅ Lightweight objects
- ✅ When fresh state needed
- ❌ Not for expensive operations

### **Scoped** - One per request
```csharp
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// In web app:
// Request 1: Gets repository instance A
// Request 2: Gets new repository instance B
// Within request: Same instance used everywhere
```

**When to use:**
- ✅ Repositories (per-request data consistency)
- ✅ Services (per-request business logic)
- ✅ DbContext (per-request database)
- ✅ Most of your services

### **Singleton** - One for entire application lifetime
```csharp
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();

// Same instance for all requests, entire app lifetime
```

**When to use:**
- ✅ Configuration
- ✅ Caches (expensive to create)
- ✅ Loggers
- ✅ Expensive resources
- ❌ NOT for stateful services
- ❌ NOT for things that should per-request

---

## Step 2: Factory Pattern

Create complex objects:

```csharp
// Instead of:
builder.Services.AddScoped<IRepository, Repository>();

// Use factory:
builder.Services.AddScoped<IRepository>(serviceProvider =>
{
    var connectionString = serviceProvider
        .GetRequiredService<IConfiguration>()
        .GetConnectionString("Default");
    
    var options = new RepositoryOptions { ConnectionString = connectionString };
    return new Repository(options);
});
```

**Benefits:**
- ✅ Complex initialization logic
- ✅ Conditional registration
- ✅ Dependency access during creation

---

## Step 3: Service Hierarchies

Services depending on services:

```csharp
public class OrderService
{
    private readonly ICustomerRepository _customers;
    private readonly IWorkOrderRepository _orders;
    private readonly IEmailService _email;
    
    public OrderService(
        ICustomerRepository customers,
        IWorkOrderRepository orders,
        IEmailService email)
    {
        _customers = customers;
        _orders = orders;
        _email = email;
    }
    
    public async Task CreateOrderAsync(int customerId, string description)
    {
        var customer = await _customers.GetAsync(customerId);
        var order = await _orders.CreateAsync(customerId, description);
        await _email.SendAsync(customer.Email, $"Order {order.Id} created");
    }
}

// Register all:
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<OrderService>();
```

---

## Step 4: Decorator Pattern

Add behavior without modifying code:

```csharp
// Original
public interface ICustomerRepository
{
    Task<Customer?> GetAsync(int id);
}

// Decorator: Add caching
public class CachedCustomerRepository : ICustomerRepository
{
    private readonly ICustomerRepository _inner;
    private readonly IMemoryCache _cache;
    
    public CachedCustomerRepository(ICustomerRepository inner, IMemoryCache cache)
    {
        _inner = inner;
        _cache = cache;
    }
    
    public async Task<Customer?> GetAsync(int id)
    {
        if (_cache.TryGetValue($"customer_{id}", out Customer? cached))
            return cached;
        
        var customer = await _inner.GetAsync(id);
        if (customer != null)
            _cache.Set($"customer_{id}", customer, TimeSpan.FromMinutes(10));
        
        return customer;
    }
}

// Extension to wrap easily
public static class ServiceCollectionExtensions
{
    public static IServiceCollection Decorate<TInterface, TDecorator>(
        this IServiceCollection services)
        where TInterface : class
        where TDecorator : class, TInterface
    {
        // Implementation here
        return services;
    }
}
```

---

## Step 5: Mini Challenge

**Refactor your Day 14 API to add:**

1. **Lifetimes:** Use appropriate lifetimes for your services
2. **Factories:** Create a factory for complex service initialization
3. **Service hierarchy:** Create an `OrderService` that uses both Customer and WorkOrder repos
4. **Decorators:** Add logging decorator to repositories
5. **Conditional registration:** Different email service for dev vs production

---

## ✅ Checklist

- [ ] Understand lifetimes (Transient, Scoped, Singleton)
- [ ] Create services with appropriate lifetimes
- [ ] Use factory pattern for complex services
- [ ] Build service hierarchies
- [ ] Implement decorator pattern
- [ ] Do conditional registration based on environment
- [ ] Code compiles without errors
- [ ] Tested that DI resolves correctly
- [ ] Compared to Day 15 Complete example

---

## 🔗 Next Steps

Day 16: **Configuration & Options Pattern** — Manage settings professionally.

---

## 📚 Resources

- <a href="https://docs.microsoft.com/dotnet/core/dependency-injection" target="_blank">Microsoft DI Documentation</a>
- <a href="https://docs.microsoft.com/dotnet/core/dependency-injection/dependency-injection-guidelines" target="_blank">DI Best Practices</a>

---

**Your API is about to get smarter.** See you on Day 16! 🚀

