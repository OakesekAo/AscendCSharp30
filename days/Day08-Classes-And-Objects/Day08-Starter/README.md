# Day 08 — Dependency Injection Basics

## 🚀 Week 2: API Sprint Begins

**Maya's Message:**
> "Week 1 is done. You know C# now. This week, we're building the heart of ServiceHub — the API. Every feature needs to be flexible, testable, and maintainable. That's what DI gives us."

Today, you'll learn **Dependency Injection** — the foundation for professional, testable code and the pattern that powers all modern .NET applications.

---

## 🎯 Learning Objectives

1. **Understand the problem:** Tightly-coupled dependencies are hard to test and maintain
2. **Learn the solution:** Dependency Injection (DI) and interfaces
3. **Implement DI in code:** Use constructor injection
4. **Register services:** Wire up DI in your application
5. **Think in dependencies:** Design classes to accept what they need
6. **Apply to ServiceHub:** Customer repository pattern

---

## 📋 Prerequisites

Before you start:
- Days 01-07 complete (comfortable with classes)
- Understand interfaces from Day 05+
- Your editor open
- ~60 minutes

---

## Setup: Create Your Day 08 Project

```bash
mkdir Day08-DependencyInjection
cd Day08-DependencyInjection
dotnet new console
```

---

## Step 1: The Problem Without DI

**Bad code (tightly-coupled):**

```csharp
class CustomerService
{
    private CustomerRepository repository = new CustomerRepository();
    
    public void AddCustomer(string name)
    {
        repository.Save(name);
    }
}
```

**Problems:**
- ❌ `CustomerService` creates its own `CustomerRepository`
- ❌ Can't use a test double (fake) in tests
- ❌ Hard to swap implementations
- ❌ Changes in `CustomerRepository` affect `CustomerService`

---

## Step 2: The Solution: Interfaces + Constructor Injection

**Good code (loosely-coupled):**

```csharp
// Define what the service needs
interface ICustomerRepository
{
    void Save(string name);
    List<string> GetAll();
}

// Implementation
class CustomerRepository : ICustomerRepository
{
    private List<string> customers = new();
    
    public void Save(string name) => customers.Add(name);
    public List<string> GetAll() => customers;
}

// Service receives what it needs
class CustomerService
{
    private ICustomerRepository repository;
    
    // Constructor injection
    public CustomerService(ICustomerRepository repo)
    {
        repository = repo;
    }
    
    public void AddCustomer(string name) => repository.Save(name);
    public List<string> ListCustomers() => repository.GetAll();
}
```

**Benefits:**
- ✅ `CustomerService` doesn't create `CustomerRepository`
- ✅ Easy to inject a test double (fake) in tests
- ✅ Easy to swap real/fake implementations
- ✅ Loosely-coupled, testable code

---

## Step 3: Constructor Injection Pattern

**Pattern:**

```csharp
class MyService
{
    private IDependency dependency;
    
    // Accept dependency via constructor
    public MyService(IDependency dep)
    {
        dependency = dep;
    }
}
```

**Usage:**

```csharp
// Create the dependency
ICustomerRepository repo = new CustomerRepository();

// Inject it into the service
CustomerService service = new CustomerService(repo);

// Use the service
service.AddCustomer("Alice");
```

---

## Step 4: DI Container (Optional but Professional)

Instead of manually creating and wiring dependencies, use a DI container:

```csharp
using Microsoft.Extensions.DependencyInjection;

// Create a DI container
var services = new ServiceCollection();

// Register dependencies
services.AddScoped<ICustomerRepository, CustomerRepository>();
services.AddScoped<CustomerService>();

// Build container
var provider = services.BuildServiceProvider();

// Get service (DI container handles wiring)
var service = provider.GetRequiredService<CustomerService>();

// Use it
service.AddCustomer("Alice");
```

**The container:**
- Automatically creates instances
- Injects dependencies
- Manages lifetimes

---

## Step 5: ServiceHub Example

**Repository interface:**

```csharp
interface ICustomerRepository
{
    void AddCustomer(Customer customer);
    Customer? GetCustomer(int id);
    List<Customer> GetAll();
}
```

**Implementation:**

```csharp
class CustomerRepository : ICustomerRepository
{
    private List<Customer> customers = new();
    
    public void AddCustomer(Customer customer) => customers.Add(customer);
    public Customer? GetCustomer(int id) => customers.FirstOrDefault(c => c.Id == id);
    public List<Customer> GetAll() => customers;
}
```

**Service using DI:**

```csharp
class CustomerService
{
    private ICustomerRepository repository;
    
    public CustomerService(ICustomerRepository repo)
    {
        repository = repo;
    }
    
    public void CreateCustomer(string name, string email)
    {
        var customer = new Customer { Name = name, Email = email };
        repository.AddCustomer(customer);
    }
    
    public List<Customer> ListAllCustomers() => repository.GetAll();
}

class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
}
```

---

## Step 6: Mini Challenge — ServiceHub with DI

**Build a ServiceHub application with:**

1. **IWorkOrderRepository interface**
   - `void AddWorkOrder(WorkOrder order)`
   - `List<WorkOrder> GetAll()`
   - `WorkOrder? GetById(int id)`

2. **WorkOrderRepository implementation**
   - Store work orders in memory

3. **WorkOrderService** 
   - Receives `IWorkOrderRepository` via constructor
   - `CreateWorkOrder()` method
   - `ListWorkOrders()` method

4. **Main program**
   - Register services with DI container
   - Get `WorkOrderService` from container
   - Create and list work orders

**Example output:**
```
Work Order Service (with DI)

Created work order #1: Gutter Cleaning
Created work order #2: Lawn Mowing

All work orders:
- #1: Gutter Cleaning
- #2: Lawn Mowing
```

---

## ✅ Checklist

- [ ] You understand what DI solves
- [ ] You can create interfaces for dependencies
- [ ] You can inject via constructor
- [ ] You understand constructor injection pattern
- [ ] You've used a DI container (ServiceCollection)
- [ ] You built the WorkOrder example with DI
- [ ] Your code is loosely-coupled and testable

---

## 🔗 Next Steps

Day 09: **Minimal APIs** — Build REST endpoints that expose your services.

---

## 📚 Resources

- <a href="https://docs.microsoft.com/dotnet/csharp/fundamentals/types/classes" target="_blank">C# Classes</a>
- <a href="https://docs.microsoft.com/dotnet/csharp/" target="_blank">C# Documentation</a>

---

## 🎬 Why This Matters for ServiceHub

This is **the** pattern you'll use all week:
- **Day 08:** Learn DI fundamentals (today)
- **Day 09:** Build API endpoints with injected services
- **Day 10-14:** Inject repositories, validators, mappers, etc.
- **Week 3:** Inject EF Core DbContext
- **Week 4:** Inject authentication, logging, configuration

**Mastering DI this week will make everything else click into place.**

---

**You've got this.** See you on Day 09! 🚀

