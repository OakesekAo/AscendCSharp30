# Day 11 — Async/Await Patterns

## 🚀 Making APIs Fast & Responsive

**Maya's Message:**
> "Synchronous code blocks. Let's make ServiceHub properly asynchronous — one of the most important patterns in modern C#."

Today, you'll learn **async/await** — how to write non-blocking code that handles multiple requests efficiently.

---

## 🎯 Learning Objectives

1. **Understand async/await** — What they do and why they matter
2. **Refactor to async** — Methods that return Tasks
3. **Async repositories** — Simulate async database calls
4. **Async services** — Call async repositories
5. **Async endpoints** — Use async in Minimal API handlers
6. **Input validation** — Validate before async operations

---

## 📋 Prerequisites

- Days 09-10 complete (understand APIs and DTOs)
- Comfortable with Task and Task<T>
- ~90 minutes

---

## Setup

```bash
mkdir Day11-AsyncAwait
cd Day11-AsyncAwait
dotnet new web
```

---

## Step 1: The Problem: Synchronous Blocking

**Bad (synchronous, blocks threads):**

```csharp
class CustomerRepository
{
    private List<Customer> customers = new();
    
    // Synchronous — blocks the thread
    public Customer GetCustomer(int id)
    {
        Thread.Sleep(100); // Simulate database delay
        return customers.FirstOrDefault(c => c.Id == id);
    }
}

// Endpoint
app.MapGet("/customers/{id}", (int id, CustomerRepository repo) =>
{
    // Thread is blocked during database call
    var customer = repo.GetCustomer(id);
    return Results.Ok(customer);
});
```

**Problem:**
- Blocks thread (wastes server resources)
- Can't handle many concurrent requests
- Scales poorly under load

---

## Step 2: The Solution: Async/Await

**Good (asynchronous, non-blocking):**

```csharp
class CustomerRepository
{
    private List<Customer> customers = new();
    
    // Asynchronous — doesn't block
    public async Task<Customer> GetCustomerAsync(int id)
    {
        await Task.Delay(100); // Simulate database delay
        return customers.FirstOrDefault(c => c.Id == id);
    }
}

// Endpoint
app.MapGet("/customers/{id}", async (int id, CustomerRepository repo) =>
{
    // Thread is NOT blocked during database call
    var customer = await repo.GetCustomerAsync(id);
    return Results.Ok(customer);
});
```

**Benefits:**
- ✅ Doesn't block threads
- ✅ Handles more concurrent requests
- ✅ Better scalability
- ✅ Responsive server

---

## Step 3: Async Method Patterns

**Pattern: Async method returning Task<T>**

```csharp
// Sync version
public Customer GetCustomer(int id)
{
    return customers.FirstOrDefault(c => c.Id == id);
}

// Async version
public async Task<Customer> GetCustomerAsync(int id)
{
    // Simulate async work (database call, API call, etc.)
    await Task.Delay(10);
    return customers.FirstOrDefault(c => c.Id == id);
}
```

**Pattern: Async method with no return (Task)**

```csharp
// Sync version
public void CreateCustomer(Customer customer)
{
    customers.Add(customer);
}

// Async version
public async Task CreateCustomerAsync(Customer customer)
{
    await Task.Delay(10); // Simulate save to database
    customers.Add(customer);
}
```

---

## Step 4: Async Repositories

**Convert your repository to async:**

```csharp
interface ICustomerRepository
{
    Task AddCustomerAsync(Customer customer);      // Was: void AddCustomer()
    Task<Customer?> GetCustomerAsync(int id);      // Was: Customer? GetCustomer()
    Task<List<Customer>> GetAllAsync();            // Was: List<Customer> GetAll()
}

class CustomerRepository : ICustomerRepository
{
    private List<Customer> customers = new();
    
    public async Task AddCustomerAsync(Customer customer)
    {
        await Task.Delay(10); // Simulate database I/O
        customers.Add(customer);
    }
    
    public async Task<Customer?> GetCustomerAsync(int id)
    {
        await Task.Delay(10);
        return customers.FirstOrDefault(c => c.Id == id);
    }
    
    public async Task<List<Customer>> GetAllAsync()
    {
        await Task.Delay(10);
        return customers;
    }
}
```

---

## Step 5: Async Services

**Services call async repositories:**

```csharp
class CustomerService
{
    private ICustomerRepository repository;
    
    public CustomerService(ICustomerRepository repo) => repository = repo;
    
    public async Task CreateCustomerAsync(Customer customer)
    {
        // Call async repository
        await repository.AddCustomerAsync(customer);
    }
    
    public async Task<Customer?> GetCustomerAsync(int id)
    {
        return await repository.GetCustomerAsync(id);
    }
    
    public async Task<List<Customer>> ListCustomersAsync()
    {
        return await repository.GetAllAsync();
    }
}
```

---

## Step 6: Async Endpoints

**Make endpoint handlers async:**

```csharp
// Async endpoint handler
app.MapGet("/customers/{id}", async (int id, CustomerService service) =>
{
    // await the async service call
    var customer = await service.GetCustomerAsync(id);
    return customer != null 
        ? Results.Ok(customer.ToResponse()) 
        : Results.NotFound();
})
.WithName("GetCustomerById")
.WithOpenApi();

// Async POST
app.MapPost("/customers", async (CreateCustomerRequest request, CustomerService service) =>
{
    var customer = request.ToCustomer();
    await service.CreateCustomerAsync(customer);    // await async call
    return Results.Created($"/customers/{customer.Id}", customer.ToResponse());
})
.WithName("CreateCustomer")
.WithOpenApi();
```

---

## Step 7: Input Validation in Async Code

**Validate BEFORE async operations:**

```csharp
app.MapPost("/customers", async (CreateCustomerRequest request, CustomerService service) =>
{
    // Validate synchronously first
    if (string.IsNullOrWhiteSpace(request.Name))
        return Results.BadRequest("Name is required");
    
    if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains("@"))
        return Results.BadRequest("Valid email required");
    
    // THEN call async service
    var customer = request.ToCustomer();
    await service.CreateCustomerAsync(customer);
    return Results.Created($"/customers/{customer.Id}", customer.ToResponse());
});
```

---

## Mini Challenge: Convert Day 10 to Async

**Make all your repositories and services async:**

1. Change repository methods to `Task<T>`
2. Change service methods to `Task<T>`
3. Make endpoint handlers `async`
4. Use `await` in handlers
5. Add validation before async calls

**Example conversion:**

```csharp
// Before
public Customer GetCustomer(int id) => repository.GetCustomer(id);

// After
public async Task<Customer?> GetCustomerAsync(int id) => 
    await repository.GetCustomerAsync(id);
```

---

## ✅ Checklist

- [ ] You understand async/await
- [ ] You can convert methods to async (Task<T>)
- [ ] You converted repositories to async
- [ ] You converted services to async
- [ ] You converted endpoint handlers to async
- [ ] You use await correctly
- [ ] You validate before async calls

---

## 🔗 Next Steps

Day 12: **Error Handling** — Handle errors properly in async code.

---

## 📚 Resources

- <a href="https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming" target="_blank">Async Programming in C#</a>
- <a href="https://learn.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming" target="_blank">Async Best Practices</a>

---

**Async/await is the foundation of modern C# APIs.** See you on Day 12! 🚀
